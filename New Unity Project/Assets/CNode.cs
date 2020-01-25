﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Parent Class of All the behaviour tree Nodes
public abstract class CNode
{
    protected CNode mParent;
    //protected CNode mCurrentNode;
    private string mNameOfNode;
    public ENodeState mCurrentNodeState = ENodeState.Failure;
    private List<CNode> childrenNodes;
    public CUI nodeUI;
    public GameObject mPrefab;


    //public abstract ENodeState RunTree();
    public abstract CNode RunTree();

    public void UpdatePrefab()
    {
        GameObject colourTemp;
        RawImage newColour;
        colourTemp = mPrefab.transform.GetChild(0).gameObject;   

        newColour = colourTemp.GetComponent<RawImage>();

        if (mCurrentNodeState == ENodeState.Success)
        {
            newColour.color = new Vector4(0, 1, 0, 0.1f);
        }
        else if (mCurrentNodeState == ENodeState.Failure)
        {
            newColour.color = new Vector4(1, 0, 0, 0.1f);
        }
        else if (mCurrentNodeState == ENodeState.Running)
        {
            newColour.color = new Vector4(1, 0.92f, 0.016f, 0.1f);
        }
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
        return childrenNodes;
    }
    public void SetChildren(List<CNode> children)
    {
        childrenNodes = children;
    }

}

public class CActionNode : CNode
{
    public delegate ENodeState mAction();
    mAction currentAction;

    public CActionNode(mAction PassedAction, string name, GameObject Prefab)
    {
        currentAction = PassedAction;
        SetName(name);
        nodeUI = new CUI();
        nodeUI.NodeName = GetName();
        mPrefab = Prefab;
        Def.SpawnNodeUI(this, mPrefab);
    }

    public override CNode RunTree()
    {
        mCurrentNodeState = currentAction();
        if (mCurrentNodeState == ENodeState.Failure)
        {                      
            UpdatePrefab();
            return this;
        }
        else if (mCurrentNodeState == ENodeState.Success)
        {
            
            UpdatePrefab();
            return this;
        }
        else
        {
            mCurrentNodeState = ENodeState.Running;
            UpdatePrefab();
            return this;
        }
    }
}

public class CSelectorNode : CNode
{
    public CNode mCurrentChildNode;
    //public List<CNode> childNodes = new List<CNode>();
    public CSelectorNode(List<CNode> PassedChildNodes, string name, GameObject Prefab)
    {
        SetChildren(PassedChildNodes);
        SetName(name);
        nodeUI = new CUI();
        nodeUI.NodeName = GetName();
        mPrefab = Prefab;
        Def.SpawnNodeUI(this, mPrefab);

        foreach (CNode i in GetChildren())
        {
            i.SetParent(this);
            i.nodeUI.xPos = nodeUI.xPos + 10;
        }
    }

    public override CNode RunTree()
    {
        mCurrentNodeState = ENodeState.Running;
        UpdatePrefab();
        foreach (CNode nodes in GetChildren())
        {
            mCurrentChildNode = nodes;
            ENodeState childnodestate = nodes.RunTree().mCurrentNodeState;
           
            if (childnodestate == ENodeState.Success)
            {
                mCurrentNodeState = ENodeState.Success;
                Debug.Log("Success " + nodes.GetName());
                UpdatePrefab();
               // nodes.UpdatePrefab();
                return nodes;
            }
            else if (childnodestate == ENodeState.Running)
            {
               // mCurrentNodeState = ENodeState.Running;
                Debug.Log("Running " + nodes.GetName());
                UpdatePrefab();
              //  nodes.UpdatePrefab();
                return nodes;
            }
        }
        mCurrentNodeState = ENodeState.Failure;
        UpdatePrefab();
        return this;
    }

}

public class CSequenceNode : CNode
{
    public CNode mCurrentChildNode;
    //public List<CNode> childNodes = new List<CNode>();
    public CSequenceNode(List<CNode> PassedChildNodes, string name, GameObject Prefab)
    {
        SetChildren(PassedChildNodes);
        SetName(name);
        nodeUI = new CUI();
        nodeUI.NodeName = GetName();
        mPrefab = Def.SpawnNodeUI(this, Prefab);
        foreach (CNode i in GetChildren())
        {
            i.SetParent(this);
        }
    }

    public override CNode RunTree()
    {
            mCurrentNodeState = ENodeState.Running;
        UpdatePrefab();
        // mCurrentNodeState = ENodeState.Running;
        foreach (CNode nodes in GetChildren())
        {
            mCurrentChildNode = nodes;
            ENodeState childnodestate = nodes.RunTree().mCurrentNodeState;
            //SpawnNodeUI(Root);
            if (childnodestate == ENodeState.Failure)
            {
                mCurrentNodeState = ENodeState.Failure;
                Debug.Log("Failure " + nodes.GetName());
                UpdatePrefab();
                //nodes.UpdatePrefab();
                return nodes;
            }
            else if (childnodestate == ENodeState.Running)
            {
               // mCurrentNodeState = ENodeState.Running;
                //mParent = this;
                Debug.Log("Running " + nodes.GetName());
                // UpdatePrefab();
               // nodes.UpdatePrefab();
                return nodes;
            }


        }
        mCurrentNodeState = ENodeState.Success;
        UpdatePrefab();
        return mCurrentChildNode;
    }

}