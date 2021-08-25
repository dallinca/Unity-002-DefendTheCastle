using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatFactoryDTC : StatFactory<EDTCStatType, EDTCStatInteractionType> {

    private static StatFactoryDTC SINGLETON;

    public static StatFactoryDTC Instance() {
        if (SINGLETON == null) {
            SINGLETON = new StatFactoryDTC();
        }

        return SINGLETON;
    }

}