namespace Goblinary.WikiData
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text.RegularExpressions;

	public partial class WorkDataSet : IWikiDataSet
	{
		private const int advancementColumns = 7;

		private enum MultiTrainerRowTypes
		{
			Member,
			Feat,
			None
		}

		public bool IsLoaded { get; set; }
		public XmlWriteMode WriteMode => XmlWriteMode.IgnoreSchema;
	    public SourceDataSet SourceDataSet { get; set; }

		private Dictionary<string, Dictionary<string, Dictionary<int, int>>> trainerFeatRankLevels = new Dictionary<string, Dictionary<string, Dictionary<int, int>>>();

		public void Import()
		{
			try
			{
				if (!SourceDataSet.IsLoaded)
					SourceDataSet.XRead();
				if (!SourceDataSet.LookupDataSet.IsLoaded)
					SourceDataSet.LookupDataSet.XRead();
				Clear();
				IsLoaded = false;
				foreach (DataTable sourceTable in SourceDataSet.Tables)
				{
					LookupDataSet.SourceWorksheetsRow workSheet = SourceDataSet.LookupDataSet.SourceWorksheets.FindByWorksheetName(sourceTable.TableName);
					if (workSheet == null)
					{
						throw new Exception($"WorkSheet '{sourceTable.TableName}' not found.");
					}
					switch (workSheet.Action)
					{
						case "Ignore":
							break;
						case "Straight":
							ImportStraight(sourceTable, workSheet);
							break;
						case "Advancement":
							ImportAdvancement(sourceTable, workSheet);
							break;
						case "Trainers":
							ImportTrainers(sourceTable, workSheet);
							break;
						case "MultiTrainers":
							ImportMultiTrainers(sourceTable, workSheet);
							break;
						default:
							throw new Exception($"Unexpected Action '{workSheet.Action}' for WorkSheet '{workSheet.WorksheetName}'.");
					}
				}
				foreach (string trainer in trainerFeatRankLevels.Keys)
				{
					foreach (string feat in trainerFeatRankLevels[trainer].Keys)
					{
						foreach (int rank in trainerFeatRankLevels[trainer][feat].Keys)
						{
							FeatRankTrainerLevels.AddFeatRankTrainerLevelsRow(feat, rank, trainer, trainerFeatRankLevels[trainer][feat][rank]);
						}
					}
				}
				IsLoaded = true;
				this.XSave();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void ImportStraight(DataTable sourceTable, LookupDataSet.SourceWorksheetsRow worksheet)
		{
			DataTable targetTable = Tables[worksheet.TargetTableName];
			string[] columnNames = worksheet.ColumnNames.Split('|');
			string pattern = @"^(?<SlotName>[^\s]*)\s*Level\s*(?<Rank>[0-9]*)\s*$";
			foreach (DataRow sourceRow in sourceTable.Rows)
			{
				DataRow targetRow = targetTable.NewRow();
				targetRow["Worksheet"] = worksheet.WorksheetName;
				if (!worksheet.IsTypeNull())
				{
					targetRow["Type"] = worksheet.Type;
				}
				for (int i = 0; i < columnNames.Length; i++)
				{
					targetRow[columnNames[i]] = sourceRow[i];
				}

				// KLUDGE Missing SlotName and Rank columns from Feat Achievements worksheet
				if (worksheet.WorksheetName == "Feat Achievements")
				{
					string displayName = sourceRow["DisplayName"].ToString();
					if (!Regex.IsMatch(displayName, pattern))
					{
						throw new Exception($"Display Name '{displayName}' does not match Pattern '{pattern}'.");
					}
					targetRow["SlotName"] = Regex.Replace(displayName, pattern, "${SlotName}");
					targetRow["Rank"] = Regex.Replace(displayName, pattern, "${Rank}");
				}
				// End kludge.

				// KLUDGE Incomplete Item Names for some Slots in Misc Gear worksheet
				if (worksheet.WorksheetName == "Misc Gear")
				{
					switch (targetRow["Slot"].ToString())
					{
						case "Holdout Weapon":
						case "Holy Symbol":
						case "Rogue Kit":
						case "Spellbook":
						case "Toolkit":
						case "Trophy Charm":
							targetRow["Name"] = $"{targetRow["Name"]} {targetRow["Slot"]}";
							break;
					}
				}
				// End kludge

				if (targetRow[targetTable.PrimaryKey[0]].ToString() != "")
				{
					targetTable.Rows.Add(targetRow);
				}
			}
		}

		private void ImportAdvancement(DataTable sourceTable, LookupDataSet.SourceWorksheetsRow worksheet)
		{
			foreach (DataRow sourceRow in sourceTable.Rows)
			{
				string featName = sourceRow[0].ToString();
				if (featName != "")
				{
					AdvancementsRow advancementsRow = Advancements.NewAdvancementsRow();
					advancementsRow.Worksheet = worksheet.WorksheetName;
					if (!worksheet.IsTypeNull())
						advancementsRow.Type = worksheet.Type;
					advancementsRow.SlotName = featName;
					advancementsRow.Description = sourceTable.Columns.Contains("description") ? sourceRow["description"].ToString() : "";
					Advancements.AddAdvancementsRow(advancementsRow);

					int Rank = 0;
					while (++Rank * advancementColumns + 1 <= sourceTable.Columns.Count)
					{
						AdvancementRanksRow advancementRanksRow = AdvancementRanks.NewAdvancementRanksRow();
						advancementRanksRow.SlotName = featName;
						advancementRanksRow.Rank = Rank.ToString();
						for (int i = 0; i < advancementColumns; i++)
						{
							advancementRanksRow[2 + i] = sourceRow[1 + (Rank - 1) * advancementColumns + i];
						}
						if (advancementRanksRow.IsExpCostNull())
						{
							break;
						}
						AdvancementRanks.AddAdvancementRanksRow(advancementRanksRow);
					}
				}
			}
		}

		private void ImportTrainers(DataTable sourceTable, LookupDataSet.SourceWorksheetsRow worksheet)
		{
			string trainer = null;
			Dictionary<string, Dictionary<int, int>> featRankLevels = null;
			foreach (DataRow row in sourceTable.Rows)
			{
				switch (row[0].ToString())
				{
					case "Trainer":
						trainer = row[1].ToString();
						if (!trainerFeatRankLevels.ContainsKey(trainer))
						{
							trainerFeatRankLevels[trainer] = new Dictionary<string, Dictionary<int, int>>();
						}
						featRankLevels = trainerFeatRankLevels[trainer];
						break;
					case "Logical Level":
					case "Building Level":
						break;
					default:
						for (int level = 1; level <= 20; level++)
						{
							if (row[level] != DBNull.Value)
							{
								string[] parts = row[level].ToString().Split('=');
								string feat = parts[0];
								int rank = int.Parse(parts[1]);
								if (!featRankLevels.ContainsKey(feat))
								{
									featRankLevels[feat] = new Dictionary<int, int>();
								}
								if (!featRankLevels[feat].ContainsKey(rank))
								{
									featRankLevels[feat][rank] = level;
								}
							}
						}
						break;
				}
			}
		}

		private void ImportMultiTrainers(DataTable sourceTable, LookupDataSet.SourceWorksheetsRow worksheet)
		{
			MultiTrainerRowTypes rowType = MultiTrainerRowTypes.None;
			Dictionary<string, decimal> trainerFactors = new Dictionary<string, decimal>();
			Dictionary<string, Dictionary<int, int>> featRankLevels = null;
			Dictionary<int, int> rankLevels = null;

			Action<DataRow> memberAction = (row) =>
			{
				trainerFactors[row[1].ToString()] = decimal.Parse(row[2].ToString());
			};

			Action<DataRow> featAction = (row) =>
			{
				string feat = row[20].ToString().Split('=')[0];
				foreach (string trainer in trainerFactors.Keys)
				{
					if (!trainerFeatRankLevels.ContainsKey(trainer))
					{
						trainerFeatRankLevels[trainer] = new Dictionary<string, Dictionary<int, int>>();
					}
					featRankLevels = trainerFeatRankLevels[trainer];
					if (!featRankLevels.ContainsKey(feat))
					{
						featRankLevels[feat] = new Dictionary<int, int>();
					}
					rankLevels = featRankLevels[feat];
					for (int level = 20; level >= 1; level--)
					{
						if (row[level].ToString() != "")
						{
							rankLevels[Convert.ToInt32(Math.Round((decimal)(int.Parse(row[level].ToString().Split('=')[1]) * trainerFactors[trainer]), MidpointRounding.AwayFromZero))] = level;
						}
					}
				}
			};

			foreach (DataRow row in sourceTable.Rows)
			{
				switch (row[0].ToString())
				{
					case "Trainer":
					case "Logical Level":
						rowType = MultiTrainerRowTypes.None;
						break;
				    case "Building Level":
				        rowType = MultiTrainerRowTypes.None;
				        break;
                    case "Member":
						rowType = MultiTrainerRowTypes.Member;
						trainerFactors = new Dictionary<string, decimal>();
						memberAction(row);
						break;
					case "Feats":
						rowType = MultiTrainerRowTypes.Feat;
						featAction(row);
						break;
					default:
						switch (rowType)
						{
							case MultiTrainerRowTypes.Member:
								memberAction(row);
								break;
							case MultiTrainerRowTypes.Feat:
								featAction(row);
								break;
							default:
								throw new Exception("Unexpected Row Type");
						}
						break;
				}
			}
		}
	}
}