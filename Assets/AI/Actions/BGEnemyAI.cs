using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction("BGEnemy AI")]
public class BGEnemyAI : RAINAction
{
    public BGEnemyAI()
    {
        actionName = "BGEnemyAI";
    }

    public override void Start(AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		Vector3 loc = Vector3.zero;

		do{
			loc = new Vector3(Mathf.Clamp(ai.Kinematic.Position.x + Random.Range(-25f,25f),-45f,700f),
			                  ai.Kinematic.Position.y,
			                  Mathf.Clamp(ai.Kinematic.Position.z + Random.Range(-10f,10f),25f,55f));

		}while((Vector3.Distance(ai.Kinematic.Position, loc) < 10f) || !ai.Navigator.OnGraph(loc));

		ai.WorkingMemory.SetItem<Vector3>("wanderTarget",loc);

        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}