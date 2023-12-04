using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //Create your Behaviour Tree here!
    }

    private void FixedUpdate()
    {
        tree?.Run();
    }
}
