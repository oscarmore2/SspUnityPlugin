using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : IJsonConfigable {

	public class ShaderParams<T>
	{
		public string fieldName;
		public T StartValue;
		public T EndValue;
		public T Step;

		public ShaderParams(string field, T startValue, T endValue, T step = default(T))
		{
			fieldName = field;
			StartValue = startValue;
			EndValue = endValue;
			Step = step;
		}
	}

	public Shader TransitionSahder { get; private set;}
	public List<ShaderParams<float>> ValueField = new List<ShaderParams<float>>();
	public List<ShaderParams<Texture>> TextureField = new List<ShaderParams<Texture>>();
	public float duration;

	public void LoadConfig(LitJson.JsonData data)
	{
		
	}

	public void SetConfig(LitJson.JsonData data)
	{
		
	}

	public void SetShader(string ShaderName)
	{
		TransitionSahder = Shader.Find (ShaderName);
	}
}
