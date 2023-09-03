using UnityEditor;
using Character.Status;

[CustomEditor(typeof(PlayerStatus))]
[CanEditMultipleObjects]
public class PlayerStatusEditor : CharacterStatusEditor 
{
    private SerializedProperty StaminaBarProperty;

    protected override void OnEnable() 
    {
        base.OnEnable();
        StaminaBarProperty = serializedObject.FindProperty(nameof(PlayerStatus.StaminaBar));
    }

    public override void OnInspectorGUI() 
    {
        var playerStatus = (PlayerStatus) target;

        serializedObject.Update();
        EditorGUILayout.LabelField("Attribute", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.FloatField("Max HP", playerStatus.BaseAttribute?.MaxHP ?? 0);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(CurrentHpProperty);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.FloatField("Max Stamina", playerStatus.PlayerHandler?.Attribute.MaxStamina ?? 0);
        EditorGUILayout.FloatField("Current Stamina", playerStatus.CurrentStamina);
        EditorGUILayout.FloatField("Regen Stamina Value", playerStatus.PlayerHandler?.Attribute.RegenStamina ?? 0);
        EditorGUILayout.IntField("Level", playerStatus.PlayerHandler?.Attribute.Level ?? 0);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(IsImmortalProperty);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("UI", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(HealthBarProperty);
        EditorGUILayout.PropertyField(StaminaBarProperty);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Event", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(OnDamagedProperty);
        EditorGUILayout.PropertyField(OnDamagedPassDamageProperty);
        EditorGUILayout.PropertyField(OnDamagedPassHitBoxProperty);
        EditorGUILayout.PropertyField(OnDiedProperty);
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
