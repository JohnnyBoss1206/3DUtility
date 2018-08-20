using UnityEngine;
using System.Collections.Generic;

/*
Boidsアルゴリズム
    群れの各個体は、群れ全体の中心(重心)へ移動しようとする。
    群れの各個体は、互いに一定以上の距離を保つ。
    群れの各個体は、群れ全体の移動速度や移動方向へ整列しようとする。
*/
namespace JohnnyGameStudio.AI.Boids
{
	/// <summary>
	/// Agentの管理クラス
	/// </summary>
	public class AgentDirector : MonoBehaviour
	{
		[SerializeField]
		private int agentNum;
		[SerializeField]
		private Agent agentPrefab;
		[SerializeField]
		private Transform centerObj;
		[SerializeField, Range(0.1f, 20f)]
		private float agentDistance = 2.1f;
		[SerializeField,Range(0,1)]
		private float turbulenceValue = 1;//乱れ値　自分の進みたい方向と中央方向への調整値
		[SerializeField, Range(1f, 10f)]
		private float agentSpeed = 1;

		[SerializeField]
		private Transform bossObj;

		private List<Agent> agentList;

		void Awake()
		{
			agentList = new List<Agent>();
			Agent agentProto = AddAgent(agentPrefab);
			for (int i = 0;i < agentNum;i++)
			{
				Agent agent = AddAgent(agentProto);
				SettingPosition(agent);
				agent.SetSpeed(agentSpeed);
			}
		}

		void Update()
		{
			Vector3 center = UpdateCenter();
			UpdateCenterMoveAgent(center);
			UpdateDistanceMoveAgent();
			UpdateAverageVectocityAgent();
		}
		/// <summary>
		/// 中央座標を更新
		/// </summary>
		private Vector3 UpdateCenter()
		{
			Vector3 center = GetCenterPosition();
			center += bossObj.position;
			center /= 2;
			centerObj.position = center;
			return center;
		}

		/// <summary>
		/// エージェントを1体生成してListへ追加
		/// </summary>
		/// <returns>生成されたClone</returns>
		private Agent AddAgent(Agent original)
		{
			Agent clone = Instantiate(original, transform) as Agent;
			agentList.Add(clone);
			clone.name = "Agent_" + agentList.Count;
			return clone;
		}
		/// <summary>
		/// 初期座標を設定
		/// </summary>
		private void SettingPosition(Agent agent)
		{
			agent.transform.position = new Vector3(Random.Range(-50f, 50f),
						  agent.transform.position.y,
						  Random.Range(-50f, 50f));
		}

		/// <summary>
		/// 群れの中心座標を取得
		/// </summary>
		/// <returns>中心座標</returns>
		private Vector3 GetCenterPosition()
		{
			Vector3 retCenter = Vector3.zero;
			for (int i = 0; i < agentList.Count; i++)
			{
				retCenter += agentList[i].transform.position;
			}
			retCenter /= agentList.Count - 1;
			return retCenter;
		}
		/// <summary>
		/// 各Agentを中央方向へ移動させるようにする
		/// </summary>
		private void UpdateCenterMoveAgent(Vector3 center)
		{
			for (int i = 0;i < agentList.Count;i++)
			{
				Vector3 dirToCenter = (center - agentList[i].transform.position).normalized;
				Vector3 direction = (agentList[i].GetVelocoty() * turbulenceValue + dirToCenter * (1 - turbulenceValue)).normalized;
				direction *= Random.Range(20f, 30f);
				agentList[i].SetVelocoty(direction);
			}
		}
		/// <summary>
		/// 各Agentを一定距離を保つようにする
		/// </summary>
		private void UpdateDistanceMoveAgent()
		{
			for (int i = 0;i < agentList.Count;i++)
			{
				for (int k = 0; k < agentList.Count; k++)
				{
					if (agentList[i] == agentList[k])
						continue;

					Vector3 diff = agentList[i].transform.position - agentList[k].transform.position;
					if (diff.magnitude < Random.Range(2, agentDistance))
					{
						Vector3 vel = diff.normalized * agentList[i].GetVelocoty().magnitude;
						agentList[i].SetVelocoty(vel);
					}
				}
			}
		}
		/// <summary>
		/// 各Agentを平均移動ベクトルに合わせようとさせる
		/// </summary>
		public void UpdateAverageVectocityAgent()
		{
			Vector3 averageVelocity = Vector3.zero;
			for (int i = 0; i < agentList.Count; i++)
			{
				averageVelocity += agentList[i].GetVelocoty();
			}
			averageVelocity /=agentList.Count;
			for (int i = 0; i < agentList.Count; i++)
			{
				Vector3 vel = agentList[i].GetVelocoty() * turbulenceValue + averageVelocity * (1f - turbulenceValue);
				agentList[i].SetVelocoty(vel);
			}
		}
	}

}
