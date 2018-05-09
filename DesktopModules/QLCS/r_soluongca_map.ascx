﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="r_soluongca_map.ascx.cs" Inherits="DotNetNuke.Modules.QLCS.r_soluongca_map" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript" language="javascript" src="<%= ModulePath + "jquery-1.6.1.min.js"%>"></script>
<script type="text/javascript" language="javascript" src="<%= ModulePath + "jquery.imagemapster.1.2.0.js"%>"></script>
<script type="text/javascript" language="javascript" src="<%= ModulePath + "Mapping.js"%>"></script>
Trước ngày
<asp:TextBox ID="txtDate" runat="server" Width="100" TabIndex="6"/>
<cc1:calendarextender id="calDate" runat="server" format="dd/MM/yyyy" popupbuttonid="txtDate" targetcontrolid="txtDate"></cc1:calendarextender>
<asp:Button ID="btnView" runat="server" Text="Xem" OnClick="btnView_Click" CssClass="button"/>
<br />

<img id="map" src="/Portals/qlcs/ban-do-chuong-trai-ca-sau.png" usemap="#realmap"/>
<map name="realmap" id="RealMap">
<area id="PA01" alt="PA01" title="" href="#" shape="rect" coords="1039,7,1088,76" style="outline:none;" target="_self" runat="server" />
<area id="PA02" alt="PA02" title="" href="#" shape="rect" coords="1091,7,1140,76" style="outline:none;" target="_self" runat="server" />
<area id="PA03" alt="PA03" title="" href="#" shape="rect" coords="1143,7,1192,76" style="outline:none;" target="_self" runat="server" />
<area id="PU01" alt="PU01" title="" href="#" shape="poly" coords="1036,7,1036,76,987,74,987,25,993,15,1000,11,1008,6" style="outline:none;" target="_self" runat="server" />
<area id="PU02" alt="PU02" title="" href="#" shape="rect" coords="1195,7,1244,76" style="outline:none;" target="_self" runat="server" />
<area id="PU03" alt="PU03" title="" href="#" shape="rect" coords="1248,7,1297,76" style="outline:none;" target="_self" runat="server" />

<area id="TP01" alt="TP01" title="" href="#" shape="rect" coords="462,719,591,882" style="outline:none;" target="_self" runat="server" />
<area id="TP02" alt="TP02" title="" href="#" shape="rect" coords="598,718,727,881" style="outline:none;" target="_self" runat="server" />
<area id="TP03" alt="TP03" title="" href="#" shape="rect" coords="742,721,871,884" style="outline:none;" target="_self" runat="server" />
<area id="TP04" alt="TP04" title="" href="#" shape="rect" coords="884,722,1013,885" style="outline:none;" target="_self" runat="server" />
<area id="TP05" alt="TP05" title="" href="#" shape="rect" coords="1022,721,1151,884" style="outline:none;" target="_self" runat="server" />
<area id="TP06" alt="TP06" title="" href="#" shape="rect" coords="1023,892,1151,1052" style="outline:none;" target="_self" runat="server" />
<area id="TP07" alt="TP07" title="" href="#" shape="rect" coords="884,893,1012,1053" style="outline:none;" target="_self" runat="server" />
<area id="TP08" alt="TP08" title="" href="#" shape="rect" coords="741,891,873,1051" style="outline:none;" target="_self" runat="server" />
<area id="TP09" alt="TP09" title="" href="#" shape="rect" coords="600,888,727,1048" style="outline:none;" target="_self" runat="server" />
<area id="TP10" alt="TP10" title="" href="#" shape="rect" coords="463,890,590,1050" style="outline:none;" target="_self" runat="server" />
<area id="TP11" alt="TP11" title="" href="#" shape="rect" coords="737,1058,867,1218" style="outline:none;" target="_self" runat="server" />
<area id="TP12" alt="TP12" title="" href="#" shape="rect" coords="882,1063,1013,1223" style="outline:none;" target="_self" runat="server" />
<area id="TP13" alt="TP13" title="" href="#" shape="rect" coords="1024,1062,1150,1222" style="outline:none;" target="_self" runat="server" />

<area id="cacon001" alt="cacon001" title="" href="#" shape="rect" coords="1177,473,1194,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon002" alt="cacon002" title="" href="#" shape="rect" coords="1194,473,1211,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon003" alt="cacon003" title="" href="#" shape="rect" coords="1210,473,1227,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon004" alt="cacon004" title="" href="#" shape="rect" coords="1227,473,1244,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon005" alt="cacon005" title="" href="#" shape="rect" coords="1243,473,1260,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon006" alt="cacon006" title="" href="#" shape="rect" coords="1260,473,1277,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon007" alt="cacon007" title="" href="#" shape="rect" coords="1277,473,1294,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon008" alt="cacon008" title="" href="#" shape="rect" coords="1293,473,1310,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon009" alt="cacon009" title="" href="#" shape="rect" coords="1310,473,1327,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon010" alt="cacon010" title="" href="#" shape="rect" coords="1327,473,1344,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon011" alt="cacon011" title="" href="#" shape="rect" coords="1343,473,1360,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon012" alt="cacon012" title="" href="#" shape="rect" coords="1360,473,1377,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon013" alt="cacon013" title="" href="#" shape="rect" coords="1376,473,1393,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon014" alt="cacon014" title="" href="#" shape="rect" coords="1393,473,1410,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon015" alt="cacon015" title="" href="#" shape="rect" coords="1409,473,1426,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon016" alt="cacon016" title="" href="#" shape="rect" coords="1426,473,1443,508" style="outline:none;" target="_self" runat="server" />
<area id="cacon017" alt="cacon017" title="" href="#" shape="rect" coords="1177,437,1194,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon018" alt="cacon018" title="" href="#" shape="rect" coords="1194,437,1211,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon019" alt="cacon019" title="" href="#" shape="rect" coords="1210,437,1227,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon020" alt="cacon020" title="" href="#" shape="rect" coords="1227,437,1244,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon021" alt="cacon021" title="" href="#" shape="rect" coords="1244,437,1261,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon022" alt="cacon022" title="" href="#" shape="rect" coords="1260,437,1277,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon023" alt="cacon023" title="" href="#" shape="rect" coords="1277,437,1294,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon024" alt="cacon024" title="" href="#" shape="rect" coords="1293,437,1310,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon025" alt="cacon025" title="" href="#" shape="rect" coords="1310,437,1327,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon026" alt="cacon026" title="" href="#" shape="rect" coords="1326,437,1343,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon027" alt="cacon027" title="" href="#" shape="rect" coords="1343,437,1360,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon028" alt="cacon028" title="" href="#" shape="rect" coords="1360,437,1377,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon029" alt="cacon029" title="" href="#" shape="rect" coords="1377,437,1394,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon030" alt="cacon030" title="" href="#" shape="rect" coords="1393,437,1410,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon031" alt="cacon031" title="" href="#" shape="rect" coords="1410,437,1427,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon032" alt="cacon032" title="" href="#" shape="rect" coords="1426,437,1443,472" style="outline:none;" target="_self" runat="server" />
<area id="cacon033" alt="cacon033" title="" href="#" shape="rect" coords="1177,400,1194,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon034" alt="cacon034" title="" href="#" shape="rect" coords="1194,400,1211,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon035" alt="cacon035" title="" href="#" shape="rect" coords="1211,400,1228,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon036" alt="cacon036" title="" href="#" shape="rect" coords="1228,400,1245,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon037" alt="cacon037" title="" href="#" shape="rect" coords="1244,400,1261,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon038" alt="cacon038" title="" href="#" shape="rect" coords="1260,400,1277,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon039" alt="cacon039" title="" href="#" shape="rect" coords="1277,400,1294,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon040" alt="cacon040" title="" href="#" shape="rect" coords="1293,400,1310,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon041" alt="cacon041" title="" href="#" shape="rect" coords="1310,400,1327,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon042" alt="cacon042" title="" href="#" shape="rect" coords="1326,400,1343,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon043" alt="cacon043" title="" href="#" shape="rect" coords="1343,400,1360,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon044" alt="cacon044" title="" href="#" shape="rect" coords="1360,400,1377,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon045" alt="cacon045" title="" href="#" shape="rect" coords="1377,400,1394,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon046" alt="cacon046" title="" href="#" shape="rect" coords="1393,400,1410,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon047" alt="cacon047" title="" href="#" shape="rect" coords="1409,400,1426,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon048" alt="cacon048" title="" href="#" shape="rect" coords="1426,400,1443,435" style="outline:none;" target="_self" runat="server" />
<area id="cacon049" alt="cacon049" title="" href="#" shape="rect" coords="1177,363,1194,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon050" alt="cacon050" title="" href="#" shape="rect" coords="1194,363,1211,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon051" alt="cacon051" title="" href="#" shape="rect" coords="1211,363,1228,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon052" alt="cacon052" title="" href="#" shape="rect" coords="1227,363,1244,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon053" alt="cacon053" title="" href="#" shape="rect" coords="1244,363,1261,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon054" alt="cacon054" title="" href="#" shape="rect" coords="1261,363,1278,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon055" alt="cacon055" title="" href="#" shape="rect" coords="1277,363,1294,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon056" alt="cacon056" title="" href="#" shape="rect" coords="1294,363,1311,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon057" alt="cacon057" title="" href="#" shape="rect" coords="1310,363,1327,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon058" alt="cacon058" title="" href="#" shape="rect" coords="1327,363,1344,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon059" alt="cacon059" title="" href="#" shape="rect" coords="1343,363,1360,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon060" alt="cacon060" title="" href="#" shape="rect" coords="1360,363,1377,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon061" alt="cacon061" title="" href="#" shape="rect" coords="1377,363,1394,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon062" alt="cacon062" title="" href="#" shape="rect" coords="1393,363,1410,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon063" alt="cacon063" title="" href="#" shape="rect" coords="1410,363,1427,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon064" alt="cacon064" title="" href="#" shape="rect" coords="1426,363,1443,398" style="outline:none;" target="_self" runat="server" />
<area id="cacon065" alt="cacon065" title="" href="#" shape="rect" coords="1177,327,1194,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon066" alt="cacon066" title="" href="#" shape="rect" coords="1194,327,1211,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon067" alt="cacon067" title="" href="#" shape="rect" coords="1210,327,1227,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon068" alt="cacon068" title="" href="#" shape="rect" coords="1227,327,1244,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon069" alt="cacon069" title="" href="#" shape="rect" coords="1244,327,1261,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon070" alt="cacon070" title="" href="#" shape="rect" coords="1261,327,1278,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon071" alt="cacon071" title="" href="#" shape="rect" coords="1277,327,1294,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon072" alt="cacon072" title="" href="#" shape="rect" coords="1293,327,1310,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon073" alt="cacon073" title="" href="#" shape="rect" coords="1310,327,1327,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon074" alt="cacon074" title="" href="#" shape="rect" coords="1326,327,1343,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon075" alt="cacon075" title="" href="#" shape="rect" coords="1343,327,1360,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon076" alt="cacon076" title="" href="#" shape="rect" coords="1360,327,1377,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon077" alt="cacon077" title="" href="#" shape="rect" coords="1376,327,1393,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon078" alt="cacon078" title="" href="#" shape="rect" coords="1393,327,1410,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon079" alt="cacon079" title="" href="#" shape="rect" coords="1409,327,1426,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon080" alt="cacon080" title="" href="#" shape="rect" coords="1426,327,1443,362" style="outline:none;" target="_self" runat="server" />
<area id="cacon081" alt="cacon081" title="" href="#" shape="rect" coords="1177,291,1194,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon082" alt="cacon082" title="" href="#" shape="rect" coords="1194,291,1211,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon083" alt="cacon083" title="" href="#" shape="rect" coords="1211,291,1228,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon084" alt="cacon084" title="" href="#" shape="rect" coords="1227,291,1244,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon085" alt="cacon085" title="" href="#" shape="rect" coords="1243,291,1260,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon086" alt="cacon086" title="" href="#" shape="rect" coords="1260,291,1277,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon087" alt="cacon087" title="" href="#" shape="rect" coords="1276,291,1293,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon088" alt="cacon088" title="" href="#" shape="rect" coords="1293,291,1310,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon089" alt="cacon089" title="" href="#" shape="rect" coords="1310,291,1327,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon090" alt="cacon090" title="" href="#" shape="rect" coords="1326,291,1343,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon091" alt="cacon091" title="" href="#" shape="rect" coords="1343,291,1360,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon092" alt="cacon092" title="" href="#" shape="rect" coords="1359,291,1376,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon093" alt="cacon093" title="" href="#" shape="rect" coords="1376,291,1393,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon094" alt="cacon094" title="" href="#" shape="rect" coords="1393,291,1410,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon095" alt="cacon095" title="" href="#" shape="rect" coords="1410,291,1427,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon096" alt="cacon096" title="" href="#" shape="rect" coords="1426,291,1443,326" style="outline:none;" target="_self" runat="server" />
<area id="cacon097" alt="cacon097" title="" href="#" shape="rect" coords="1177,255,1194,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon098" alt="cacon098" title="" href="#" shape="rect" coords="1194,255,1211,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon099" alt="cacon099" title="" href="#" shape="rect" coords="1211,255,1228,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon100" alt="cacon100" title="" href="#" shape="rect" coords="1228,255,1244,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon101" alt="cacon101" title="" href="#" shape="rect" coords="1244,255,1260,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon102" alt="cacon102" title="" href="#" shape="rect" coords="1260,255,1276,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon103" alt="cacon103" title="" href="#" shape="rect" coords="1277,255,1293,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon104" alt="cacon104" title="" href="#" shape="rect" coords="1293,255,1309,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon105" alt="cacon105" title="" href="#" shape="rect" coords="1310,255,1326,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon106" alt="cacon106" title="" href="#" shape="rect" coords="1327,255,1343,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon107" alt="cacon107" title="" href="#" shape="rect" coords="1343,255,1359,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon108" alt="cacon108" title="" href="#" shape="rect" coords="1359,255,1375,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon109" alt="cacon109" title="" href="#" shape="rect" coords="1376,255,1392,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon110" alt="cacon110" title="" href="#" shape="rect" coords="1393,255,1409,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon111" alt="cacon111" title="" href="#" shape="rect" coords="1410,255,1426,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon112" alt="cacon112" title="" href="#" shape="rect" coords="1427,255,1443,290" style="outline:none;" target="_self" runat="server" />
<area id="cacon113" alt="cacon113" title="" href="#" shape="rect" coords="1177,218,1193,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon114" alt="cacon114" title="" href="#" shape="rect" coords="1194,218,1210,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon115" alt="cacon115" title="" href="#" shape="rect" coords="1211,218,1227,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon116" alt="cacon116" title="" href="#" shape="rect" coords="1228,218,1244,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon117" alt="cacon117" title="" href="#" shape="rect" coords="1244,218,1260,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon118" alt="cacon118" title="" href="#" shape="rect" coords="1260,218,1276,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon119" alt="cacon119" title="" href="#" shape="rect" coords="1277,218,1293,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon120" alt="cacon120" title="" href="#" shape="rect" coords="1293,218,1309,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon121" alt="cacon121" title="" href="#" shape="rect" coords="1310,218,1326,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon122" alt="cacon122" title="" href="#" shape="rect" coords="1327,218,1343,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon123" alt="cacon123" title="" href="#" shape="rect" coords="1343,218,1359,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon124" alt="cacon124" title="" href="#" shape="rect" coords="1359,218,1375,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon125" alt="cacon125" title="" href="#" shape="rect" coords="1376,218,1392,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon126" alt="cacon126" title="" href="#" shape="rect" coords="1393,218,1409,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon127" alt="cacon127" title="" href="#" shape="rect" coords="1410,218,1426,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon128" alt="cacon128" title="" href="#" shape="rect" coords="1427,218,1443,253" style="outline:none;" target="_self" runat="server" />
<area id="cacon129" alt="cacon129" title="" href="#" shape="rect" coords="1177,182,1193,217" style="outline:none;" target="_self" runat="server" />
<area id="cacon130" alt="cacon130" title="" href="#" shape="rect" coords="1194,182,1210,217" style="outline:none;" target="_self" runat="server" />
<area id="cacon131" alt="cacon131" title="" href="#" shape="rect" coords="1211,182,1227,217" style="outline:none;" target="_self" runat="server" />
<area id="cacon132" alt="cacon132" title="" href="#" shape="rect" coords="1228,182,1244,217" style="outline:none;" target="_self" runat="server" />
<area id="cacon133" alt="cacon133" title="" href="#" shape="rect" coords="1244,182,1260,217" style="outline:none;" target="_self" runat="server" />
<area id="cacon134" alt="cacon134" title="" href="#" shape="rect" coords="1260,182,1276,217" style="outline:none;" target="_self" runat="server" />
<area id="cacon135" alt="cacon135" title="" href="#" shape="rect" coords="1277,182,1293,217" style="outline:none;" target="_self" runat="server" />
<area id="cacon136" alt="cacon136" title="" href="#" shape="rect" coords="1293,182,1309,217" style="outline:none;" target="_self" runat="server" />

<area id="TG01" alt="TG01" title="" href="#" shape="rect" coords="1181,533,1219,604" style="outline:none;" target="_self" runat="server" />
<area id="TG02" alt="TG02" title="" href="#" shape="rect" coords="1225,533,1263,604" style="outline:none;" target="_self" runat="server" />
<area id="TG03" alt="TG03" title="" href="#" shape="rect" coords="1269,533,1307,604" style="outline:none;" target="_self" runat="server" />
<area id="TG04" alt="TG04" title="" href="#" shape="rect" coords="1312,533,1350,604" style="outline:none;" target="_self" runat="server" />
<area id="TG05" alt="TG05" title="" href="#" shape="rect" coords="1356,533,1394,604" style="outline:none;" target="_self" runat="server" />
<area id="TG06" alt="TG06" title="" href="#" shape="rect" coords="1399,533,1437,604" style="outline:none;" target="_self" runat="server" />
<area id="TG07" alt="TG07" title="" href="#" shape="rect" coords="1181,608,1219,679" style="outline:none;" target="_self" runat="server" />
<area id="TG08" alt="TG08" title="" href="#" shape="rect" coords="1225,609,1263,680" style="outline:none;" target="_self" runat="server" />
<area id="TG09" alt="TG09" title="" href="#" shape="rect" coords="1269,609,1307,680" style="outline:none;" target="_self" runat="server" />
<area id="TG10" alt="TG10" title="" href="#" shape="rect" coords="1312,609,1350,680" style="outline:none;" target="_self" runat="server" />
<area id="TG11" alt="TG11" title="" href="#" shape="rect" coords="1356,609,1394,680" style="outline:none;" target="_self" runat="server" />
<area id="TG12" alt="TG12" title="" href="#" shape="rect" coords="1399,609,1437,680" style="outline:none;" target="_self" runat="server" />
<area id="TG13" alt="TG13" title="" href="#" shape="rect" coords="1181,684,1219,755" style="outline:none;" target="_self" runat="server" />
<area id="TG14" alt="TG14" title="" href="#" shape="rect" coords="1225,684,1263,755" style="outline:none;" target="_self" runat="server" />
<area id="TG15" alt="TG15" title="" href="#" shape="rect" coords="1269,685,1307,756" style="outline:none;" target="_self" runat="server" />
<area id="TG16" alt="TG16" title="" href="#" shape="rect" coords="1312,685,1350,756" style="outline:none;" target="_self" runat="server" />
<area id="TG17" alt="TG17" title="" href="#" shape="rect" coords="1356,685,1394,756" style="outline:none;" target="_self" runat="server" />
<area id="TG18" alt="TG18" title="" href="#" shape="rect" coords="1399,685,1437,756" style="outline:none;" target="_self" runat="server" />
<area id="TG19" alt="TG19" title="" href="#" shape="rect" coords="1181,761,1219,832" style="outline:none;" target="_self" runat="server" />
<area id="TG20" alt="TG20" title="" href="#" shape="rect" coords="1225,761,1263,832" style="outline:none;" target="_self" runat="server" />
<area id="TG21" alt="TG21" title="" href="#" shape="rect" coords="1269,761,1307,832" style="outline:none;" target="_self" runat="server" />
<area id="TG22" alt="TG22" title="" href="#" shape="rect" coords="1312,761,1350,832" style="outline:none;" target="_self" runat="server" />
<area id="TG23" alt="TG23" title="" href="#" shape="rect" coords="1356,761,1394,832" style="outline:none;" target="_self" runat="server" />
<area id="TG24" alt="TG24" title="" href="#" shape="rect" coords="1399,761,1437,832" style="outline:none;" target="_self" runat="server" />
<area id="TG25" alt="TG25" title="" href="#" shape="rect" coords="1181,837,1219,908" style="outline:none;" target="_self" runat="server" />
<area id="TG26" alt="TG26" title="" href="#" shape="rect" coords="1225,837,1263,908" style="outline:none;" target="_self" runat="server" />
<area id="TG27" alt="TG27" title="" href="#" shape="rect" coords="1181,913,1219,984" style="outline:none;" target="_self" runat="server" />
<area id="TG28" alt="TG28" title="" href="#" shape="rect" coords="1225,913,1263,984" style="outline:none;" target="_self" runat="server" />

<area id="KSS01" alt="KSS01" title="" href="#" shape="poly" coords="1674,15,1673,258,1875,260,1891,243,1867,46,1849,25,1825,13,1816,11" style="outline:none;" target="_self" runat="server" />
<area id="KSS02" alt="KSS02" title="" href="#" shape="rect" coords="1468,12,1660,259" style="outline:none;" target="_self" runat="server" />
<area id="KSS03" alt="KSS03" title="" href="#" shape="rect" coords="1469,270,1661,517" style="outline:none;" target="_self" runat="server" />
<area id="KSS04" alt="KSS04" title="" href="#" shape="poly" coords="1673,272,1672,286,1672,502,1689,517,1895,516,1911,496,1891,286,1873,272,1859,270" style="outline:none;" target="_self" runat="server" />
<area id="KSS05" alt="KSS05" title="" href="#" shape="poly" coords="1721,551,1721,800,1728,823,1740,827,1826,791,1876,741,1902,695,1915,651,1923,615,1921,578,1915,540,1900,526,1745,526,1729,534,1722,542" style="outline:none;" target="_self" runat="server" />
<area id="KSS06" alt="KSS06" title="" href="#" shape="rect" coords="1470,527,1708,723" style="outline:none;" target="_self" runat="server" />
<area id="KSS07" alt="KSS07" title="" href="#" shape="poly" coords="1613,733,1690,734,1706,743,1709,828,1702,839,1612,874,1599,867,1598,750,1603,739" style="outline:none;" target="_self" runat="server" />
<area id="KSS08" alt="KSS08" title="" href="#" shape="poly" coords="1467,752,1469,905,1481,919,1490,921,1579,887,1586,876,1586,748,1576,736,1563,732,1487,732,1475,739" style="outline:none;" target="_self" runat="server" />

<area id="DDA" alt="DDA" title="" href="#" shape="poly" coords="741,382,741,525,751,531,841,529,843,677,1139,675,1136,382" style="outline:none;" target="_self" runat="server" />
</map>
