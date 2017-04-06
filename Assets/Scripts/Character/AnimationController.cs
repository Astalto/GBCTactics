using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MoveableCharacter))]
[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    public Animator m_animator;
    public MoveableCharacter m_characterScript;

    public void Update()
    {
        m_animator.SetBool("Selected", m_characterScript.m_isSelected);
        m_animator.SetBool("MovingUp", m_characterScript.m_movingUp);
        m_animator.SetBool("MovingDown", m_characterScript.m_movingDown);
        m_animator.SetBool("MovingLeft", m_characterScript.m_movingLeft);
        m_animator.SetBool("MovingRight", m_characterScript.m_movingRight);

    }



}
