<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CopeWP4 - COMPARE</title>
    <link rel="shortcut icon" href="Images/favicon.ico" />
    <link href="UserStyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmDefault" runat="server" autocomplete="off">
     
        
		        
		<table cellspacing="0" cellpadding="0" border="0" width="100%" style="font-family: Arial;">
            <tr>
		        <td valign="middle" align="center">
                    <table    style="width:500px; align-self:center; background-color:#ececec; "  >
			            <tr>
				            <td colspan="2"  align="center" 
                                style=" height: 35px;background-color:#339933; ">
			    	            <asp:Label BorderStyle="None" ForeColor="White" 
							            Runat="server" id="lblLogin" Font-Bold="True" Font-Italic="True" 
                                    Font-Size="Large" Width="275px">COPE WP4 - COMPARE <br /> (DUMMY DATABASE)</asp:Label>
				            </td>
						    
			            </tr>
			
			            <tr style="height: 30px;">
				            <td  style="height: 30px; width:35%" align="right">
						            <asp:Label BorderStyle="None" runat="server" ID="lblUsername" Font-Bold="True"  
                                        Text="Username" Font-Size="Medium" />
                            </td>
                            <td   style="width:65%" align="left">
                                <asp:TextBox BorderStyle="None" runat="server" ID="txtUsername" 
                                        MaxLength="20" Width="75%" autocomplete="off"></asp:TextBox>
                            </td>
			            </tr>
			            <tr  style="height: 30px;">
				            <td align="right">
					            <asp:Label BorderStyle="None" runat="server" ID="lblPassword" Font-Bold="True" Font-Size="Medium" Text="Password"  />
					        </td>
                            <td align="left">
                                <asp:TextBox BorderStyle="None" runat="server" ID="txtPassword" 
                                        TextMode="Password"  MaxLength="20" Width="75%" autocomplete="off" />
                                <br />
                            </td>
			            </tr>
			            <tr>
				            <td colspan="2" align="right"  >
					            <asp:Button id="cmdLogin" runat="server" Text="Login" Font-Size="Medium"  
                                    Width="120px" onclick="cmdLogin_Click"></asp:Button>
						    						    						    
                            </td>
			            </tr>
			            <tr>
				            <td colspan="2" >
					            <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" Width="95%" ForeColor="Red" />
                                <asp:Label runat="server" ID="lblExtra" Font-Size="Small" Width="0%" Visible="false" />
				            </td>
			            </tr>
					    
		            </table>
                </td>
            </tr>
        </table>
			   
    
    
    </form>
</body>
</html>
