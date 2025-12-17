using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "FideMon/Attack")]
public class Attack : ScriptableObject
{
    [SerializeField] private string attackName = "Ataque";
    [SerializeField] private string description = "Descripción del ataque";
    [SerializeField] private float baseDamage = 20f;
    [SerializeField] private AttackType attackType = AttackType.Normal;
    [SerializeField] private Sprite attackIcon;
    [SerializeField] private GameObject attackEffect;
    [SerializeField] private Color attackColor = Color.white;
    [SerializeField] private AudioClip attackSound;
    
    public string AttackName => attackName;
    public string Description => description;
    public float BaseDamage => baseDamage;
    public AttackType Type => attackType;
    public Sprite AttackIcon => attackIcon;
    public GameObject AttackEffect => attackEffect;
    public Color AttackColor => attackColor;
    public AudioClip AttackSound => attackSound;
}

public enum AttackType
{
    Normal,
    Fuego,
    Agua,
    Planta,
    Electrico,
    Hielo,
    Tierra,
    Volador,
    Psiquico,
    Veneno
}