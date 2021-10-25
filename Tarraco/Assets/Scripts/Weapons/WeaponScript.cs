using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeaponScript
{
    public void SetOnHandColliders();
    public void SetOnFloorColliders();
    public void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c);
    public void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c);
}
