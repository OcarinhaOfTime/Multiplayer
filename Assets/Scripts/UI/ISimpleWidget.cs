using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISimpleWidget {
    void ShowForced();
    IEnumerator Show();
    IEnumerator Hide();
}
