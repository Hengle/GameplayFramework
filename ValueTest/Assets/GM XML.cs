using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

public partial class GM
{
    string helpMsg = "";
    private IEnumerator LoadXML()
    {
        WWW www = new WWW(PathPrefix.WWWstreamingAssets + Application.streamingAssetsPath + "/Help.xml");
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            var buffer = www.bytes.SkipBOMIfHave();

            XElement XE = XElement.Parse(System.Text.Encoding.UTF8.GetString(buffer));
            helpMsg = XE.Element("Help").Value;
        }
        else
        {
            Debug.Log(www.error);
        }
    }
}