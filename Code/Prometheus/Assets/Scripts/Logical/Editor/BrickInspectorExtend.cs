using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Brick))]
public class BrickInspectorExtend : Editor
{

    string id = "1000001";
    string power = "0";
    string lv = "1";

    public override void OnInspectorGUI()
    {
        Brick brick = base.target as Brick;

        base.OnInspectorGUI();

        int row = serializedObject.FindProperty("_row").intValue;
        int column = serializedObject.FindProperty("_column").intValue;

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("monster id: ");

        id = GUILayout.TextField(id);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("power: ");

        power = GUILayout.TextField(power);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("lv ");

        lv = GUILayout.TextField(lv);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create Monster"))
        {
            GameItemFactory.Instance.CreateMonster(int.Parse(power), ulong.Parse(id), int.Parse(lv), brick);
        }

    }
}
