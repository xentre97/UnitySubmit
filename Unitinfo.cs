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
    public Side side; //�v���C���[1��0�A�v���C���[2��1
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

    //�ړ��\�͈͎擾
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
        int go = (side == 0) ? 1 : -1; //�v���C���[�Ȃ�1,�łȂ����-1

        if (type == UnitType.Spear) //���̈ړ��͈�
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
        if (type == UnitType.Cavalry) //�R���̈ړ��͈�
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
        if (type == UnitType.Bow) //�|���̈ړ��͈�
        {
            vec.Add(new Vector2Int(0, 1 * go));
            vec.Add(new Vector2Int(0, -1 * go));
            vec.Add(new Vector2Int(1 * go, 0));
            vec.Add(new Vector2Int(-1 * go, 0));
        }
        if (type == UnitType.Ji) //�����̈ړ��͈�
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
            Vector2Int checkPos = Pos + item; //�����܂Ő���

            if (!isCheckable(units, checkPos) || units[checkPos.x, checkPos.y] != null) continue;

            ret1.Add(checkPos);
        }
        return ret1;
    }
    //�U���\�͈͎擾
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
        int go = (side == 0) ? 1 : -1; //�v���C���[�Ȃ�1,�łȂ����-1

        if (type == UnitType.Spear) //���̍U���͈�
        {
            vec.Add(new Vector2Int(0, 1 * go));
            vec.Add(new Vector2Int(1 * go, 2*go));
            vec.Add(new Vector2Int(-1 * go, 2*go));
            vec.Add(new Vector2Int(1 * go, 3 * go));
            vec.Add(new Vector2Int(-1 * go, 3 * go));
        }
        if (type == UnitType.Cavalry) //�R���̍U���͈�
        {
            vec.Add(new Vector2Int(0, 1 * go));
            vec.Add(new Vector2Int(0, 2 * go));
            vec.Add(new Vector2Int(0, 3 * go));
            vec.Add(new Vector2Int(0, 4 * go));
            vec.Add(new Vector2Int(0, 5 * go));
        }
        if (type == UnitType.Bow) //�|���̍U���͈�
        {
            vec.Add(new Vector2Int(0, 2 * go));
            vec.Add(new Vector2Int(0, 3 * go));
            vec.Add(new Vector2Int(0, 4 * go));
            vec.Add(new Vector2Int(1 * go, 3*go));
            vec.Add(new Vector2Int(-1 * go, 3*go));
        }
        if (type == UnitType.Ji) //�����̍U���͈�
        {
            vec.Add(new Vector2Int(1 * go, 1 * go));   
            vec.Add(new Vector2Int(-1 * go, 1 * go));
            vec.Add(new Vector2Int(1 * go, 2 * go));
            vec.Add(new Vector2Int(-1 * go, 2 * go));
            vec.Add(new Vector2Int(0, 3*go));
        }

        foreach (var item in vec)
        {
            Vector2Int checkPos = Pos + item; //�����܂Ő���

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
        // ���݂̃��j�b�g���������鑤���s�����Ă�����
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
                        // �_���[�W�����B�����ł͒P����HP�������������
                        target.HP -= 1; // �_���[�W�ʂ͒����\
                        if (target.HP <= 0)
                        {
                            Destroy(target.gameObject); // �Q�[���I�u�W�F�N�g�̍폜

                            // �����f�[�^�̍폜
                            units[pos.x, pos.y] = null; // �z����̎Q�Ƃ��N���A
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