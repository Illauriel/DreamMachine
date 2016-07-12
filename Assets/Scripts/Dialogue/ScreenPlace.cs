using UnityEngine;
using UnityEngine.UI;
using System.Collections;


[System.Serializable]
public class ScreenPlace {
	public Image image;			  	//sprite drawn
	//public Image[] ghosts;			//ghost copies of sprites for transition
	//public int[] tagplaces;			//a long list of default placements
	public string placeName;		  	//names of placement points
	public string active_tag;	  	//tags of currently drawn sprites
	public string active_proprties; 
}
