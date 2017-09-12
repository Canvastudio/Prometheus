using UnityEngine;
using System.Collections;

public class BoneHelper {

	public static Transform GetTran(Transform tranRoot, string name) {
	
		Transform[] trans = tranRoot.GetComponentsInChildren<Transform>(true);

		Transform returnTran = null;

		string boneStr = "";

		for(int i = 0; i < trans.Length; i++) {
		
			if(trans[i].name == name) {
			
				returnTran = trans[i];
			
			}

			boneStr += trans[i].name + "," + "\n";

		
		}	

		return returnTran;
	
	}

    public static Transform[] GetTranArray(Transform tranRoot, string[] names) {
    
        Transform[] tranArray = new Transform[names.Length];

        for(int i = 0; i < names.Length; i++) {
        
            tranArray[i] = GetTran(tranRoot, names[i]);
        
        }

        return tranArray;

    
    }

    public static GameObject GetBaseTran(Transform tranRoot) {
    
        Transform currentRoot = tranRoot;

        while(true) {
        
            if(currentRoot.parent == null) return currentRoot.gameObject;
        
        }

        return currentRoot.gameObject;
    
    }

}

//public enum BoneType {
//
//	Bip001,
//	Bip001_Pelvis,
//	Bip001 L Thigh,
//	Bip001 L Calf,
//	Bip001 L HorseLink,
//	Bip001 L Foot,
//	Bip001 R Thigh,
//	Bip001 R Calf,
//	Bip001 R HorseLink,
//	Bip001 R Foot,
//	Bip001 Spine,
//	Bip001 Spine1,
//	Bip001 Spine2,
//	Bip001 L Clavicle,
//	Bip001 L UpperArm,
//	Bip001 L Forearm,
//	Bip001 L Hand,
//	Bip001 Neck,
//	Bip001 Neck1,
//	Bip001 Head,
//	Bip001 Ponytail1,
//	Bip001 Ponytail11,
//	Bip001 Ponytail2,
//	Bip001 Ponytail21,
//	Bone L Eye,
//	Bone R Eye,
//	Bip001 R Clavicle,
//	Bip001 R UpperArm,
//	Bip001 R Forearm,
//	Bip001 R Hand,
//	Bip001 Tail,
//	Bip001 Tail1,
//	Bip001 Tail2,
//	Bip001 Tail3,
//
//}