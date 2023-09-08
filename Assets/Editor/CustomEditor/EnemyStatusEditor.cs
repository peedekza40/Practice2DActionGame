using UnityEditor;
using Character.Status;

[CustomEditor(typeof(EnemyStatus))]
[CanEditMultipleObjects]
public class EnemyStatusEditor : CharacterStatusEditor 
{
    private SerializedProperty TypeProperty;
    private SerializedProperty AttributeProperty;

    protected override void OnEnable() 
    {
        base.OnEnable();
        TypeProperty = serializedObject.FindProperty(nameof(EnemyStatus.Type));
        AttributeProperty = serializedObject.FindProperty(nameof(EnemyStatus.Attribute));
    }

    public override void OnInspectorGUI() 
    {
        var enemyStatus = (EnemyStatus) target;

        serializedObject.Update();
        EditorGUILayout.LabelField("Attribute", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(AttributeProperty);
        EditorGUILayout.PropertyField(TypeProperty);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.FloatField("Max HP", enemyStatus.BaseAttribute?.MaxHP ?? 0);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(CurrentHpProperty);
        EditorGUILayout.PropertyField(IsImmortalProperty);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("UI", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(HealthBarProperty);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(OnDamagedProperty);
        EditorGUILayout.PropertyField(OnDamagedPassDamageProperty);
        EditorGUILayout.PropertyField(OnDamagedPassHitBoxProperty);
        EditorGUILayout.PropertyField(OnDiedProperty);
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();

        // base.DrawDefaultInspector();
    }
}
