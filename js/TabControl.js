var m_oTS; 
function getTabStrip()
{
    //i=0;
    //while (i<dnn.controls.controls.length)
    //{
        //window.alert(dnn.controls.controls.length);
        //i=i+1;
    //}
	if (m_oTS == null)
		m_oTS = dnn.controls.controls[m_sNS + '_MyDNNTabStrip'];
	return m_oTS;
}

function statusFunc(result, ctx, req)
{
	if (req.completed)
	{
		addMethodToList('>>> CALLBACK - Size: ' + req.postData.length);
		addMethodToList('--------------------------------------------------\n ' + req.postData + '\n--------------------------------------------------');
		addMethodToList('>>> Render Size: ' + result.length);
	}
}

function addMethodToList(s)
{
	var oCtl = $(m_sNS + '_txtMethods');
	var ary = oCtl.value.split('\n');
	ary[ary.length-1] = ary.length + '. ' + s + '\n';
	oCtl.value = ary.join('\n');

}
	
function tabClick(evt, arg)
{
	addMethodToList(arg.get_tab().tabId + ' clicked');
}

function selectedIndexChanged(evt, element)
{
	updateTabs();
}

function nextStep()
{
	var oTS = getTabStrip();
	var oTab = oTS.setSelectedIndex(oTS.selectedIndex += 1);
	oTab.enabled = true;
	updateTabs();
}

function prevStep()
{
	var oTS = getTabStrip();
	var oTab = oTS.setSelectedIndex(oTS.selectedIndex -= 1);
	updateTabs();
}

function updateTabs()
{
	var oTS = getTabStrip();

	//enable, disable navigation buttons	
	if (oTS.tabIds.length - 1 > oTS.selectedIndex)
	{
		setTab(oTS.selectedIndex + 1, true);	//enable next tab
		oTS.resetTab(oTS.tabIds[oTS.selectedIndex + 1]); //reset next tab, to allow it to be re-retrieved from callback...  up to you whether you want your wizard to do this
		
		if (oTS.tabIds.length - 1 > oTS.selectedIndex + 1)
			setTab(oTS.selectedIndex+2, false);	//disable tab following next		

		$('btnNext').disabled = false;
	}
	else
		$('btnNext').disabled = true;
	
	if (oTS.selectedIndex > 0)
		$('btnPrev').disabled = false;
	else
		$('btnPrev').disabled = true;
	
}

function setTab(iIdx, bEnabled)
{
	var oTS = getTabStrip();
	var oTab = oTS.tabs[oTS.tabIds[iIdx]];
	oTab.enabled = bEnabled;
	oTab.assignCss();
	return oTab;
}

