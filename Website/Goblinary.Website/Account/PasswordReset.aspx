<%@ Page Title="Password Reset" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PasswordReset.aspx.cs" Inherits="Goblinary.Website.PasswordReset" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2>Password Reset</h2>
            </hgroup>
		</div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
	<asp:PasswordRecovery OnVerifyingUser="validateUserEmail" 
		  SuccessText="Your password was successfully reset and emailed to you - it will have been sent from do-not-reply@goblinary.com."
		  runat="server" ID="PasswordRecovery" 
		  UserNameFailureText="Username not found.">
		<MailDefinition IsBodyHtml="true" BodyFileName="/Account/PasswordRecoveryEmail.txt" 
			   From="do-not-reply@goblinary.com" 
			   Subject="Goblinary Password Reset" 
			   Priority="High">
		</MailDefinition>
		<UserNameTemplate>
			<p>If you have forgotten your password, you can use the form below to reset it:</p>
			<dl>
				<dt>Username</dt>
				<dd>
					<asp:TextBox ID="Username" runat="server" />
				</dd>
				<dt>Email</dt>
				<dd>
					<asp:TextBox ValidationGroup="PasswordRecovery" runat="server" ID="EmailAddressTextBox">
					</asp:TextBox>
				</dd>
				<dt></dt>
				<dd>
					<asp:Button ID="submit" CausesValidation="true" ValidationGroup="PasswordRecovery" runat="server" CommandName="Submit" Text="Submit" />
				</dd>
				<dt></dt>
				<dd>
					<p class="Error"><asp:Literal ID="ErrorLiteral" runat="server"></asp:Literal>
					</p>
				</dd>
			</dl>
		</UserNameTemplate>
	</asp:PasswordRecovery>
</asp:Content>