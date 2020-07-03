using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Parent Class of All the behaviour tree Nodes
public abstract class CNode
{
    private CNode mParent;
    private string mNameOfNode;
    public ENodeState mCurrentNodeState = ENodeState.Failure;    
    public CUI mNodeUI;
    public GameObject mPrefab;
    public List<CNode> childNodes;

    //public abstract ENodeState RunTree();
    public abstract CNode RunTree();

    public void UpdatePrefab()
    {
        GameObject colourTemp;
        RawImage newColour;
        //colourTemp = mPrefab.transform.GetChild(0).gameObject;

        //newColour = colourTemp.GetComponent<RawImage>();

        //if (mCurrentNodeState == ENodeState.Success)
        //{
        //    newColour.color = new Vector4(0, 1, 0, 0.1f);
        //}
        //else if (mCurrentNodeState == ENodeState.Failure)
        //{
        //    newColour.color = new Vector4(1, 0, 0, 0.1f);
        //}
        //else if (mCurrentNodeState == ENodeState.Running)
        //{
        //    newColour.color = new Vector4(1, 0.92f, 0.016f, 0.1f);
        //}
    }

    public void ShowTreeHud()
    {
        mPrefab = Def.SpawnNodeUI(this, mPrefab);
    }



    public void ResetTreeStates()
    {
        mCurrentNodeState = ENodeState.Failure;
    }

    public string GetName()
    {
        return mNameOfNode;
    }
    public void SetName(string name)
    {
        mNameOfNode = name;
    }
    public CNode GetParent()
    {
        return mParent;
    }
    public void SetParent(CNode parent)
    {
        mParent = parent;
    }
    public List<CNode> GetChildren()
    {
        return childNodes;
    }
  


}

public abstract class CDecoratorNode : CNode
{   
    public void SetChildren(CNode children)
    {
        childNodes = new List<CNode>();
        childNodes.Add(children);
    }
}

public abstract class CCompositeNode : CNode
{
    public void SetChildren(List<CNode> children)
    {
        childNodes = children;
    }
}

public class CActionNode : CNode
{
    public delegate ENodeState mAction();
    mAction currentAction;

    public CActionNode(mAction PassedAction, string name)
    {
        currentAction = PassedAction;
        SetName(name);
        mNodeUI = new CUI();
        mNodeUI.NodeName = GetName();
    }

    public override CNode RunTree()
    {
        mCurrentNodeState = currentAction();
        ////UpdatePrefab();
        if (mCurrentNodeState == ENodeState.Failure)
        {
            return this;
        }
        else if (mCurrentNodeState == ENodeState.Success)
        {
            return this;
        }
        else
        {
            mCurrentNodeState = ENodeState.Running;
            //UpdatePrefab();
            return this;
        }
    }
}

public class CSelectorNode : CCompositeNode
{
    public CNode mCurrentChildNode;
    //public List<CNode> childNodes = new List<CNode>();
    public CSelectorNode(List<CNode> PassedChildNodes, string name)
    {
        SetChildren(PassedChildNodes);
        SetName(name);
        mNodeUI = new CUI();
        mNodeUI.NodeName = GetName();
        //mNodeUI.xPos = pos.x;
        //mNodeUI.yPos = pos.y;
        //mPrefab = Prefab;
        //mPrefab = Def.SpawnNodeUI(this, mPrefab);
        //mPrefab.SetActive(false);

        foreach (CNode i in GetChildren())
        {
            i.SetParent(this);
        }
    }

    public override CNode RunTree()
    {
        mCurrentNodeState = ENodeState.Running;
        //UpdatePrefab();
        foreach (CNode nodes in GetChildren())
        {
            mCurrentChildNode = nodes;
            ENodeState childnodestate = nodes.RunTree().mCurrentNodeState;

            if (childnodestate == ENodeState.Success)
            {
                mCurrentNodeState = ENodeState.Success;
                Debug.Log("Success " + nodes.GetName());
                //UpdatePrefab();
                return nodes;
            }
            else if (childnodestate == ENodeState.Running)
            {
                // mCurrentNodeState = ENodeState.Running;
                Debug.Log("Running " + nodes.GetName());
                //UpdatePrefab();
                return nodes;
            }
        }
        mCurrentNodeState = ENodeState.Failure;
        //UpdatePrefab();
        return this;
    }

}

public class CSequenceNode : CCompositeNode
{
    public CNode mCurrentChildNode;
    //public List<CNode> childNodes = new List<CNode>();
    public CSequenceNode(List<CNode> PassedChildNodes, string name)
    {
        SetChildren(PassedChildNodes);
        SetName(name + "Sequence");
        mNodeUI = new CUI();
        mNodeUI.NodeName = GetName() + "Sequence";
        //mNodeUI.xPos = pos.x;
        //mNodeUI.yPos = pos.y;
        //mPrefab = Prefab;
        //mPrefab = Def.SpawnNodeUI(this, mPrefab);
        //mPrefab.SetActive(false);
        foreach (CNode i in GetChildren())
        {
            i.SetParent(this);
        }
    }

    public override CNode RunTree()
    {
        mCurrentNodeState = ENodeState.Running;
        //UpdatePrefab();
        foreach (CNode nodes in GetChildren())
        {
            mCurrentChildNode = nodes;
            ENodeState childnodestate = nodes.RunTree().mCurrentNodeState;

            if (childnodestate == ENodeState.Failure)
            {
                mCurrentNodeState = ENodeState.Failure;
                Debug.Log("Failure " + nodes.GetName());
                //UpdatePrefab();
                return nodes;
            }
            else if (childnodestate == ENodeState.Running)
            {
                Debug.Log("Running " + nodes.GetName());
                mCurrentNodeState = ENodeState.Running;
                //UpdatePrefab();
                return nodes;
            }
        }
        mCurrentNodeState = ENodeState.Success;
        //UpdatePrefab();
        return mCurrentChildNode;
    }

}

public class CTimerNode : CDecoratorNode
{
    public delegate ENodeState mAction();
    public float mTimer;
    public float mTimerDelay;

    public CTimerNode(CNode childNode, string name, float delay)
    {
        SetChildren(childNode);
        GetChildren()[0].SetParent(this);
        SetName(name + "Timer");
        mNodeUI = new CUI();
        mNodeUI.NodeName = GetName() + "Timer";
        mTimer = Time.time;
        mTimerDelay = delay;
    }

    public override CNode RunTree()
    {
        if (mTimer + mTimerDelay < Time.time)
        {
            mTimer = Time.time;
            GetChildren()[0].RunTree();
            mCurrentNodeState = ENodeState.Success;
            //UpdatePrefab();
            return GetChildren()[0];
        }

        mCurrentNodeState = ENodeState.Failure;
        //UpdatePrefab();
        return this;
    }
}



public class CInverterNode : CDecoratorNode
{
    public delegate ENodeState mAction();

    public CInverterNode(CNode childNode, string name)
    {
        SetChildren(childNode);
        GetChildren()[0].SetParent(this);
        SetName(name + "Inverter");
    }

    public override CNode RunTree()
    {
        GetChildren()[0].RunTree();
        if (GetChildren()[0].mCurrentNodeState == ENodeState.Failure)
        {
            mCurrentNodeState = ENodeState.Success;
            return this;
        }
        else if (GetChildren()[0].mCurrentNodeState == ENodeState.Success)
        {
            mCurrentNodeState = ENodeState.Failure;
            return this;
        }

        mCurrentNodeState = ENodeState.Running;
        return this;
    }
}