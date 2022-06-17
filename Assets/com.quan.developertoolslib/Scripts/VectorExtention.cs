using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeveloperToolsLib {
    public static class VectorExtention {

        /// <summary>求角度 及前后左右方位</summary>
        public static float[] checkTargetDirForMe(this Transform me, Transform target) {
            Vector3 dir = target.position - me.position; //位置差，方向  
            //方式1   点乘  
            //点积的计算方式为: a·b =| a |·| b | cos < a,b > 其中 | a | 和 | b | 表示向量的模 。  
            float dot = Vector3.Dot(me.forward, dir.normalized);//点乘判断前后：dot >0在前，<0在后
            float dot1 = Vector3.Dot(me.right, dir.normalized);//点乘判断左右： dot1>0在右，<0在左
            float angle = Mathf.Acos(Vector3.Dot(me.forward.normalized, dir.normalized)) * Mathf.Rad2Deg;//通过点乘求出夹角  
            return new float[] { dot, dot1, angle };

            /*
                //方式2   叉乘  
                //叉乘满足右手准则  公式：模长|c|=|a||b|sin<a,b>    
                Vector3 cross = Vector3.Cross(me.forward, dir.normalized);//叉乘判断左右：cross.y>0在左，<0在右   
                Vector3 cross1 = Vector3.Cross(me.right, dir.normalized); //叉乘判断前后：cross.y>0在前，<0在后   
                angle = Mathf.Asin(Vector3.Distance(Vector3.zero, Vector3.Cross(me.forward.normalized, dir.normalized))) * Mathf.Rad2Deg;  
                return new float[]{cross.y, cross1.y, angle};
            */
        }

        /// <summary>
        /// 计算两向量之间的夹角
        /// </summary>
        /// <param name="_from"></param>
        /// <param name="_to"></param>
        /// <returns></returns>
        public static float DotToAngle(Vector3 _from, Vector3 _to) {
            return Mathf.Acos(Vector3.Dot(_from.normalized, _to.normalized)) * Mathf.Rad2Deg;
        }
    }
}