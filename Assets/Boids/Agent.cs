using UnityEngine;
using System.Collections;

namespace JohnnyGameStudio.AI.Boids
{
	[RequireComponent(typeof(Rigidbody))]
	public class Agent : MonoBehaviour
	{
		private Rigidbody rigid;
		private float speed;
		void Awake()
		{
			rigid = GetComponent<Rigidbody>();
			rigid.velocity = Vector3.forward;
		}
		/// <summary>
		/// ベロシティーを取得
		/// </summary>
		public Vector3 GetVelocoty()
		{
			return rigid.velocity.normalized;
		}
		/// <summary>
		/// ベロシティーをセット
		/// </summary>
		public void SetVelocoty(Vector3 v)
		{
			rigid.velocity = v * speed;
		}
		public void SetSpeed(float speed)
		{
			this.speed = speed;
		}
	}

}