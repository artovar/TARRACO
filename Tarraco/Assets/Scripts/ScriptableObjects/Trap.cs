using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Trap", menuName = "Tarraco/Trap")]
public class Trap : ScriptableObject {
    public string trapName;
    public enum trapType {DAMAGE, SLOWDOWN};
    public trapType type;
}