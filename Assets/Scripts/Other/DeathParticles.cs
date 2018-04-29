using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticles : MonoBehaviour {
    public List<Color> colors;
    ParticleSystem _particleSystem;
    ParticleSystem.Particle[] particles;
    ParticleSystem.MainModule mainModule;
    private int totalParticles;
    Color color;

    bool dothing = false;
    // Use this for initialization
    void Start () {
        _particleSystem = this.GetComponent<ParticleSystem>();
        mainModule = _particleSystem.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(colors[0],colors[1]);
    }
	
	// Update is called once per frame
	//void Update () {
		
	//}

 //   private void LateUpdate()
 //   {
 //       if (dothing)
 //       {
 //           totalParticles = _particleSystem.GetParticles(particles);
 //           for (int i = 0; i < totalParticles; i++)
 //           {
 //               color = colors[Random.Range(0, colors.Count - 1)];
 //               particles[i].startColor = color;
 //           }

 //           _particleSystem.SetParticles(particles, totalParticles);
 //           dothing = false;
 //       }
      
 //   }


 //   private void OnEnable()
 //   {
 //       dothing = true;
 //   }

}
