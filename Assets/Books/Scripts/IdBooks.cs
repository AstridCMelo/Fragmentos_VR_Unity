using UnityEngine;

[CreateAssetMenu(fileName = "NewIdBook", menuName = "Scriptable Objects/IdBooks")]
public class IdBooks : ScriptableObject
{
    [SerializeField] private string titulo;
    private char InitialLetter => titulo[0];
    private int idBook => (int)InitialLetter;

    public int IdBook { get { return idBook; } }
}
