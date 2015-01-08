<%@ Page Title="" Language="C#" MasterPageFile="~/Login.master" AutoEventWireup="true"
    CodeFile="AutoURLRedirect.aspx.cs" Inherits="AutoURLRedirect" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <center>
        <div id="center" style="width: 100%; height: 70%;" align="center">
            <div style="text-align: center">
                <br />
                <br />
                <br />
                <br />
                <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Verdana"
                    Text="Home" Font-Size="8pt" ForeColor="Navy" NavigateUrl="https://www.vcmpartners.com"></asp:HyperLink>
            </div>
            <br />
            <div id="contactBox">
                <table id="TABLE1" style="left: -13px; position: relative; top: 37px; height: 200px">
                    <tr>
                        <td style="width: 152px; text-align: left">
                            <ul>
                                <li><span>NewYork Office</span> </li>
                                <li><span>Address: </span>44 Wall Street, 21st Floor, New York, NY 10005 </li>
                                <li><span>Telephone:</span> +1-646-688-7500 </li>
                                <li><span>Fax:</span> +1-646-688-7499 </li>
                                <li><span>Email: </span><a href="mailto:info@vcmpartners.com">info@vcmpartners.com</a>
                                </li>
                            </ul>
                        </td>
                        <td style="width: 152px; text-align: left">
                            <ul style="left: 4px; position: relative; top: -3px; height: 91px">
                                <li><span>London Office</span> </li>
                                <li>1st Floor, 4 Broadgate, London EC2M 2QY</li>
                                <li>+44 (0)20-7398-2800</li>
                                <li>+44 (0)20 7398 2801</li>
                                <li><a href="mailto:info@vcmpartners.com">info@vcmpartners.com</a> </li>
                            </ul>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </center>
</asp:Content>
