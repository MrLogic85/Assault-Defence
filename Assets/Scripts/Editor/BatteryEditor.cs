using UnityEditor;

[CustomEditor(typeof(Battery))]
public class BatteryEditor : Editor
{
    private Battery battery;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        battery = target as Battery;
        EditorGUILayout.LabelField("Store power " + battery.GetPower());
    }
}
