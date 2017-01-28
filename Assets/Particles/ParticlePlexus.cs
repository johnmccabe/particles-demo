using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePlexus : MonoBehaviour {

	public float maxDistance = 1.0f;

	new ParticleSystem particleSystem;
	ParticleSystem.Particle[] particles;

	ParticleSystem.MainModule particleSystemMainModule;

	public LineRenderer lineRendererTemplate;
	List<LineRenderer> lineRenderers = new List<LineRenderer>();

	Transform _transform;

	void Start () {
		particleSystem = GetComponent<ParticleSystem> ();
		particleSystemMainModule = particleSystem.main;
	}
	
	void LiveUpdate () {
		int maxParticles = particleSystemMainModule.maxParticles;

		if (particles == null || particles.Length < maxParticles){
			particles = new ParticleSystem.Particle[maxParticles];
		}

		particleSystem.GetParticles (particles);
		int particleCount = particleSystem.particleCount;

		float maxDistanceSqr = maxDistance * maxDistance;

		int lrIndex = 0;
		int lineRendererCount = lineRenderers.Count;

		// Could use partitioning here
		for (int i = 0; i < particleCount; i++) {
			Vector3 p1_position = particles [i].position;
			for (int j = i + 1; j < particleCount; j++) {
				Vector3 p2_position = particles [j].position;
				float distanceSqr = Vector3.SqrMagnitude (p1_position - p2_position);

				if (distanceSqr <= maxDistanceSqr) {

					LineRenderer lr;

					if (lrIndex == lineRendererCount) {
						lr = Instantiate (lineRendererTemplate, _transform, false);
						lineRenderers.Add (lr);
						lineRendererCount++;
					}

					lr = lineRenderers [lrIndex];

					lr.enabled = true;

					lr.SetPosition (0, p1_position);
					lr.SetPosition (1, p2_position);
				}
			}
		}

		for (int i = lrIndex; i < lineRenderers.Count; i++) {
			lineRenderers [i].enabled = false;
		}
	}
}
