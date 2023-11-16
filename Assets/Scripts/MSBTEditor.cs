using MsbtEditor;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MSBTEditor : MonoBehaviour
{
    public void UpdateValues()
    {
        try
        {
            StringBuilder sb = new StringBuilder();

            List<string> row = new List<string>();

            IEntry ent = null;
            if (MSBTLoader.MSBT.HasLabels)
            {
                sb.AppendLine("Label,String");
                for (int i = 0; i < MSBTLoader.MSBT.TXT2.NumberOfStrings; i++)
                {
                    Label lbl = (Label)MSBTLoader.MSBT.LBL1.Labels[i];
                    ent = MSBTLoader.MSBT.LBL1.Labels[i];

                    row.Add(ent.ToString());
                    row.Add("\"" + lbl.String.ToString(Encoding.Unicode).Replace("\"", "\"\"") + "\"");
                    sb.AppendLine(string.Join(",", row.ToArray()));
                    row.Clear();
                }
            }
            else
            {
                sb.AppendLine("Index,String");
                for (int i = 0; i < MSBTLoader.MSBT.TXT2.NumberOfStrings; i++)
                {
                    MsbtEditor.String str = (MsbtEditor.String)MSBTLoader.MSBT.TXT2.Strings[i];
                    ent = MSBTLoader.MSBT.LBL1.Labels[i];

                    row.Add((ent.Index + 1).ToString());
                    row.Add("\"" + str.ToString(Encoding.Unicode).Replace("\"", "\"\"") + "\"");
                    sb.AppendLine(string.Join(",", row.ToArray()));
                    row.Clear();
                }
            }

            Debug.Log(sb.ToString());
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
