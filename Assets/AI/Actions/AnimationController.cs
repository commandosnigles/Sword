using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction("Enemy Animation Controller")]
public class AnimationController : RAINAction
{

    public AnimationController()
    {
        actionName = "AnimationController";
    }
	private string _currentState = "Walk";
	private string _detectObj = "";
	private string _nearObj = "";
    public override void Start(AI ai)
    {
		ai.WorkingMemory.SetItem("varState", _currentState);
		_detectObj = ai.WorkingMemory.GetItem<string>("detectTarget");
		_nearObj = ai.WorkingMemory.GetItem<string>("nearTarget");
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {

		_detectObj = ai.WorkingMemory.GetItem<string>("detectTarget");
		_nearObj = ai.WorkingMemory.GetItem<string>("nearTarget");
		//Debug.Log(_detectObj);
		//Debug.Log(ai.Animator.IsStatePlaying("Attack"));
		if(ai.WorkingMemory.GetItem<float>("health") != 0){
			if(_detectObj=="" && _nearObj == ""){
				if(!ai.Animator.IsStatePlaying("Attack")){
					_currentState = "Walk";
					ai.WorkingMemory.SetItem("varState", _currentState);
				}
			}else if(_detectObj!="" && _nearObj ==""){
				if(!ai.Animator.IsStatePlaying("Attack")){
					_currentState = "Run";
					ai.WorkingMemory.SetItem("varState", _currentState);
				}
			}else if(_detectObj!="" && _nearObj !=""){
				_currentState = "Attack";
				ai.WorkingMemory.SetItem("varState", _currentState);
			}
		}else{
			_currentState = "Dead";
			ai.WorkingMemory.SetItem("varState", _currentState);
		}
	
        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }


}