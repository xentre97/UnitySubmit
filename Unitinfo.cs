using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public enum UnitType
{
    None    = 0,
    Spear,
    Cavalry,
    Bow,
    Ji
}

public enum FieldStatus
{
    None    = -1,
    OnBoard,
    Captured,
}

public enum Side
{
    P1 = 0,
    P2 = 1,
}

public class Unitinfo : MonoBehaviour
{
    public int HP = 10;
    public bool unitMoved = false;
    public Side side; //プレイヤー1は0、プレイヤー2は1
    public UnitType unitType;
    public FieldStatus fieldStatus;

    public const float SelectUnitY = 13f;
    public const float UnselectUnitY = 6f;
    public Text hptext;

    public Vector2Int Pos;

    void Start()
    {

    }

    void Update()
    {

    }
    public void unitMovedReset()
    {
        unitMoved = false;
    }

    public void Init(int num, int unittype, GameObject tile, Vector2Int pos, Control control)
    {
        side = (Side)num;
        unitType = (UnitType)unittype;
        fieldStatus = FieldStatus.OnBoard;
        Move(tile,pos,control);
        unitMoved = false;

    }

    public void Move(GameObject tile, Vector2Int tileindex, Control control)
    {
        if(!unitMoved)
        {
            Vector3 pos = tile.transform.position;
            pos.y = UnselectUnitY;
            this.transform.position = pos;
            Pos = tileindex;
            unitMoved = true;
        }

    }
    public void Select(bool select)
    {
        Vector3 pos =transform.position;
        if (select==true)
        {
            pos.y = SelectUnitY;
        }
        else if(select==false)
        {
            pos.y = UnselectUnitY;
        }
        transform.position = pos;
    }

    //移動可能範囲取得
    public List<Vector2Int> GetMovableTiles(Unitinfo[,] units)
    {
        List<Vector2Int> ret = new List<Vector2Int>();
        if(unitType == UnitType.Spear)
        {
            ret = GetMovableTiles1(units, UnitType.Spear);
        }
        if(unitType == UnitType.Cavalry)
        {
            ret = GetMovableTiles1(units, UnitType.Cavalry);
        }
        if(unitType == UnitType.Bow)
        {
            ret = GetMovableTiles1(units, UnitType.Bow);
        }
        if (unitType == UnitType.Ji)
        {
            ret = GetMovableTiles1(units, UnitType.Ji);
        }

        return ret;

    }

    List<Vector2Int> GetMovableTiles1(Unitinfo[,] units, UnitType type)
    {
        List<Vector2Int> ret1 = new List<Vector2Int>();
        List<Vector2Int> vec = new List<Vector2Int>();
        int go = (side == 0) ? 1 : -1; //プレイヤーなら1,でなければ-1

        if (type == UnitType.Spear) //槍の移動範囲
        {
            vec.Add(new Vector2Int(0, 1 * go));
            vec.Add(new Vector2Int(0, -1 * go));
            vec.Add(new Vector2Int(1 * go, 0));
            vec.Add(new Vector2Int(-1 * go, 0));
            vec.Add(new Vector2Int(1 * go, 1 * go));
            vec.Add(new Vector2Int(1 * go, -1 * go));
            vec.Add(new Vector2Int(-1 * go, 1 * go));
            vec.Add(new Vector2Int(-1 * go, -1 * go));
        }
        if (type == UnitType.Cavalry) //騎兵の移動範囲
        {
            vec.Add(new Vector2Int(0, 1 * go));
            vec.Add(new Vector2Int(0, -1 * go));
            vec.Add(new Vector2Int(1 * go, 0));
            vec.Add(new Vector2Int(-1 * go, 0));
            vec.Add(new Vector2Int(0, 2 * go));
            vec.Add(new Vector2Int(0, -2 * go));
            vec.Add(new Vector2Int(2 * go, 0));
            vec.Add(new Vector2Int(-2 * go, 0));
        }
        if (type == UnitType.Bow) //弓兵の移動範囲
        {
            vec.Add(new Vector2Int(0, 1 * go));
            vec.Add(new Vector2Int(0, -1 * go));
            vec.Add(new Vector2Int(1 * go, 0));
            vec.Add(new Vector2Int(-1 * go, 0));
        }
        if (type == UnitType.Ji) //戟兵の移動範囲
        {
            vec.Add(new Vector2Int(1 * go, 0));
            vec.Add(new Vector2Int(-1 * go, 0));
            vec.Add(new Vector2Int(0, 1 * go));
            vec.Add(new Vector2Int(1*go, 1 * go));
            vec.Add(new Vector2Int(1*go, -1 * go));
            vec.Add(new Vector2Int(-1 * go, 1*go));
            vec.Add(new Vector2Int(-1 * go, -1*go));
        }

        foreach (var item in vec)
        {
            Vector2Int checkPos = Pos + item; //ここまで正常

            if (!isCheckable(units, checkPos) || units[checkPos.x, checkPos.y] != null) continue;

            ret1.Add(checkPos);
        }
        return ret1;
    }
    //攻撃可能範囲取得
    public List<Vector2Int> GetAttackableunits(Unitinfo[,] units)
    {
        List<Vector2Int> ret = new List<Vector2Int>();
        if (unitType == UnitType.Spear)
        {
            ret = GetAttacableunits1(units, UnitType.Spear);
        }
        if (unitType == UnitType.Cavalry)
        {
            ret = GetAttacableunits1(units, UnitType.Cavalry);
        }
        if (unitType == UnitType.Bow)
        {
            ret = GetAttacableunits1(units, UnitType.Bow);
        }
        if (unitType == UnitType.Ji)
        {
            ret = GetAttacableunits1(units, UnitType.Ji);
        }

        return ret;

    }

    List<Vector2Int> GetAttacableunits1(Unitinfo[,] units, UnitType type)
    {
        List<Vector2Int> ret1 = new List<Vector2Int>();
        List<Vector2Int> vec = new List<Vector2Int>();
        int go = (side == 0) ? 1 : -1; //プレイヤーなら1,でなければ-1

        if (type == UnitType.Spear) //槍の攻撃範囲
        {
            vec.Add(new Vector2Int(0, 1 * go));
            vec.Add(new Vector2Int(1 * go, 2*go));
            vec.Add(new Vector2Int(-1 * go, 2*go));
            vec.Add(new Vector2Int(1 * go, 3 * go));
            vec.Add(new Vector2Int(-1 * go, 3 * go));
        }
        if (type == UnitType.Cavalry) //騎兵の攻撃範囲
        {
            vec.Add(new Vector2Int(0, 1 * go));
            vec.Add(new Vector2Int(0, 2 * go));
            vec.Add(new Vector2Int(0, 3 * go));
            vec.Add(new Vector2Int(0, 4 * go));
            vec.Add(new Vector2Int(0, 5 * go));
        }
        if (type == UnitType.Bow) //弓兵の攻撃範囲
        {
            vec.Add(new Vector2Int(0, 2 * go));
            vec.Add(new Vector2Int(0, 3 * go));
            vec.Add(new Vector2Int(0, 4 * go));
            vec.Add(new Vector2Int(1 * go, 3*go));
            vec.Add(new Vector2Int(-1 * go, 3*go));
        }
        if (type == UnitType.Ji) //戟兵の攻撃範囲
        {
            vec.Add(new Vector2Int(1 * go, 1 * go));   
            vec.Add(new Vector2Int(-1 * go, 1 * go));
            vec.Add(new Vector2Int(1 * go, 2 * go));
            vec.Add(new Vector2Int(-1 * go, 2 * go));
            vec.Add(new Vector2Int(0, 3*go));
        }

        foreach (var item in vec)
        {
            Vector2Int checkPos = Pos + item; //ここまで正常

            if (!attackCheckable(units, checkPos)) continue;

            ret1.Add(checkPos);
        }
        return ret1;
    }

    bool isCheckable(Unitinfo[,] units, Vector2Int idx)
    {
        if (side == Side.P1)
        {
            if (idx.x < 0 || units.GetLength(0) <= idx.x
            || idx.y < 0 || units.GetLength(1) / 2 <= idx.y)
            {
                return false;
            }
            return true;
        }
        else
        {
            if (idx.x <0  || units.GetLength(0) <= idx.x
            || idx.y < 3 || units.GetLength(1) <= idx.y)
            {
                return false;
            }
            return true;
        }
        
    }
    bool attackCheckable(Unitinfo[,] units, Vector2Int idx)
    {
        if (idx.x < 0 || units.GetLength(0) <= idx.x
||          idx.y < 0 || units.GetLength(1) <= idx.y)
        {
            return false;
        }
        return true;
    }
    public void Damage(Unitinfo[,] units)
    {
        // 現在のユニットが所属する側が行動していた側
        foreach (var unit in units)
        {
            if (unit != null && unit.side == this.side)
            {
                List<Vector2Int> attackablePositions = unit.GetAttackableunits(units);

                foreach (Vector2Int pos in attackablePositions)
                {
                    Unitinfo target = units[pos.x, pos.y];
                    if (target != null && target.side != this.side)
                    {
                        // ダメージ処理。ここでは単純にHPを減少させる例
                        target.HP -= 1; // ダメージ量は調整可能
                        if (target.HP <= 0)
                        {
                            Destroy(target.gameObject); // ゲームオブジェクトの削除

                            // 内部データの削除
                            units[pos.x, pos.y] = null; // 配列内の参照をクリア
                        }
                    }
                }
            }
        }
    }
    public void HPShow(TextMeshProUGUI inst)
    {
        Vector3 Pos = gameObject.transform.position;
        inst.text = HP.ToString();
        TextMeshProUGUI tile = Instantiate(inst, Pos, Quaternion.identity);
    }

}