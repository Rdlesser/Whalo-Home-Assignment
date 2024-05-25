﻿using System.Collections;
using System.Collections.Generic;

namespace Scripts {

    public static class Extensions {

        public static void Shuffle(this IList list) {
            
            var rand = new System.Random();
            for(int i = list.Count - 1; i > 0; --i) {
                var j = rand.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
        
        
        public static bool IsNullOrEmpty<T>(this IList<T> collection) {
            
            return collection == null || collection.Count == 0;
        }

        public static bool IsNullOrEmpty(this ICollection collection) {

            return collection == null || collection.Count == 0;
        }
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) {
            
            return collection == null || collection.Count == 0;
        }

    }

}
