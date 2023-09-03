using Character.Status;
using UnityEditor;

[CustomEditor(typeof(CharacterStatus), true)]
[CanEditMultipleObjects]
public class CharacterStatusEditor : Editor 
{
    protected SerializedProperty CurrentHpProperty;
    protected SerializedProperty IsImmortalProperty;
    protected SerializedProperty HealthBarProperty;
    protected SerializedProperty OnDamagedProperty;
    protected SerializedProperty OnDamagedPassDamageProperty;
    protected SerializedProperty OnDamagedPassHitBoxProperty;
    protected SerializedProperty OnDiedProperty;

    protected virtual void OnEnable() 
    {
        CurrentHpProperty = serializedObject.FindProperty(nameof(CharacterStatus.CurrentHP));
        IsImmortalProperty = serializedObject.FindProperty(nameof(CharacterStatus.IsImmortal));
        HealthBarProperty = serializedObject.FindProperty(nameof(CharacterStatus.HealthBar));
        OnDamagedProperty = serializedObject.FindProperty(nameof(CharacterStatus.OnDamaged));
        OnDamagedPassDamageProperty = serializedObject.FindProperty(nameof(CharacterStatus.OnDamagedPassDamage));
        OnDamagedPassHitBoxProperty = serializedObject.FindProperty(nameof(CharacterStatus.OnDamagedPassHitBox));
        OnDiedProperty = serializedObject.FindProperty(nameof(CharacterStatus.OnDied));
    }
}