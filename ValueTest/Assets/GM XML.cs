using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Assets.DB;
using Assets.DB.MikuDataTableAdapters;
using Devart.Data.SQLite;
using UnityEngine;

public partial class GM
{
    public string HelpMsg { get ;private set; }
    public SQLiteHelper DB { get; private set; }
    public MikuData DataSet { get; private set; } = new MikuData();
    public static Dictionary<string, ModelTemplate> ModelDic => modelDic;

    private IEnumerator LoadXML()
    {
        WWW www = new WWW(PathPrefix.WWWstreamingAssets + Application.streamingAssetsPath + "/Help.xml");
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            var buffer = www.bytes.SkipBOMIfHave();
            XElement XE = XElement.Parse(Encoding.UTF8.GetString(buffer));
            HelpMsg = XE.Value;
        }
        else
        {
            Debug.Log(www.error);
        }
    }
    static Dictionary<string, ModelTemplate> modelDic = new Dictionary<string, ModelTemplate>();
    private IEnumerator LoadDB()
    {
        var path = $"data source=" + Application.streamingAssetsPath + "/MikuMikuFight.db3";
        DB = new SQLiteHelper(path);
        yield return null;
        var reader = DB.ReadFullTable("ModelList");
        while (reader.Read())
        {
            ModelTemplate model = new ModelTemplate();
            model.ID = reader.GetInt32(0);
            model.Name = reader.GetString(1);
            if (reader.IsDBNull(2))
            {
                model.Path = $"Character/{model.Name}/{model.Name}";
            }
            else
            {
                model.Path = reader.GetString(2);
            }

            ModelDic.Add(model.Name, model);
        }

        CharacterTemplateTableAdapter ap = new CharacterTemplateTableAdapter();
        ap.Connection = DB.DbConnection;
        ap.Fill(DataSet.CharacterTemplate);
        DataInfoTemplateTableAdapter ap2 = new DataInfoTemplateTableAdapter();
        ap2.Connection = DB.DbConnection;
        ap2.Fill(DataSet.DataInfoTemplate);

        ModelListTableAdapter ap3 = new ModelListTableAdapter();
        ap3.Connection = DB.DbConnection;
        ap3.Fill(DataSet.ModelList);
    }
}