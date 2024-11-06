using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static UnityEngine.UI.CanvasScaler;

public class Control : MonoBehaviour
{
    public bool Awake = false;
    AudioSource audioSource;
    public AudioClip winning;
    public AudioClip choice;
    public AudioClip put;
    public AudioClip change;
    public TextMeshProUGUI P1Win;
    public TextMeshProUGUI P2Win;
    public TextMeshProUGUI P1TurnTXT;
    public TextMeshProUGUI P2TurnTXT;

    public TextMeshProUGUI HPshow;


    [SerializeField] GameObject prefabtile;
    [SerializeField] List<GameObject> prefabUnits;
    [SerializeField] GameObject cameraParent;

    [SerializeField] bool hasMoved = false; //�ړ��ς݂��̔���
    bool Movable = false; //�ړ��\���̔���
    bool reChoice = false; //�đI���̔���

    [SerializeField] int P1HowManyMoved = 0; //�^�[�����ɉ��񓮂������iMAX3�j
    [SerializeField] int P2HowManyMoved = 0;
    [SerializeField] int P1Num = 3; 
    [SerializeField] int P2Num =3; //�
    [SerializeField] int Turn = 1;
    [SerializeField] int HitCount;
    public int in1; public int in2; public int in3;
    public int in4; public int in5; public int in6;
   

    public bool P1turn = true;
    public bool P2turn = false;
    bool player = false;

    /*int[,] boardSetting =
    {
        {0,2,0,0,13,0 },
        {0,1,0,0,11,0 },
        {0,4,0,0,12,0 }
    };*/

    Dictionary<Vector2Int, GameObject> tiles;
    Unitinfo[,] units; //�S���j�b�g
    public Unitinfo selectUnit; //�I�����ꂽ���j�b�g

    Dictionary<Vector2Int, GameObject> movableTiles; //�ړ��\�^�C��
    Dictionary<Vector2Int, GameObject> attackableTiles; //�U���\�^�C��

    [SerializeField] GameObject prefabCursor; //�J�[�\���v���n�u(�ړ�
    List<GameObject> cursors; //�J�[�\���I�u�W�F�N�g���X�g(�ړ�
    [SerializeField] GameObject AttackCursor;
    List<GameObject> attackCursors;

    [SerializeField] Material num1;
    [SerializeField] Material num2;
    [SerializeField] Material num3;
    [SerializeField] Material num4;
    [SerializeField] Material num5;
    [SerializeField] Material num6;
    [SerializeField] Material num7;
    [SerializeField] Material num8;
    [SerializeField] Material num9;
    [SerializeField] Material num10;
    [SerializeField] Material num0;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (P1Win != null) //�����Ńe�L�X�g���A�N�e�B�u
        {
            P1Win.gameObject.SetActive(false);
        }
        if (P2Win != null)
        {
            P2Win.gameObject.SetActive(false);
        }
        if(P1TurnTXT != null && P2TurnTXT != null)
        {
            P1TurnTXT.gameObject.SetActive(false);
            P2TurnTXT.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        GameObject tile =null;
        Unitinfo unit =null;//������
        if(Input.GetMouseButtonDown(0) && Awake)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                Unitinfo hitunit =hit.transform.GetComponent<Unitinfo>();

                if (tiles.ContainsValue(hit.transform.gameObject))
                {
                    tile = hit.transform.gameObject;
                    Vector2Int Vec2pos = tiles.FirstOrDefault(x => x.Value == tile).Key;
                    Vector2Int movableIndex =
                            movableTiles.FirstOrDefault(x => x.Value == tile).Key;

                    if ((!hasMoved && !Movable) || reChoice) //�I���̂��߂̃N���b�N
                    {
                        reChoice = false;
                        foreach (var item in tiles)
                        {
                            if (item.Value == tile)
                            {
                                unit = units[item.Key.x, item.Key.y];
                            }
                        }
                        if (unit == null)//�G�̏ꍇ�̃��Z�b�g�\
                        {
                            cursorReset(); 
                            preventDoubleSelect();
                            return;
                        }
                    }
                    if(!hasMoved && Movable &&!movableTiles.ContainsKey(Vec2pos))
                    {
                        reChoice = false;
                        foreach (var item in tiles)
                        {
                            if (item.Value == tile)
                            {
                                unit = units[item.Key.x, item.Key.y];
                            }
                        }
                        if (unit == null)//�G�̏ꍇ�̃��Z�b�g�\
                        {
                            cursorReset();
                            preventDoubleSelect();
                            return;
                        }
                    }
                    if (!player && units[Vec2pos.x, Vec2pos.y] == null && !Movable && !hasMoved)
                    {
                        Debug.Log("�G�̂܂��N���b�N");
                        cursorReset();
                        preventDoubleSelect();
                    }//����Ȃ��H
                    if (player && tile && selectUnit && movableTiles.ContainsKey(Vec2pos) && Movable && !hasMoved)
                    {
                        if (!selectUnit.unitMoved) //�ʏ�ړ�����
                        {
                            moveUnit(selectUnit, Vec2pos);
                            selectUnit.Select(false);
                            Movable = false;
                            return;
                        }
                    }

                    if (tile && selectUnit && Movable && Vec2pos ==  selectUnit.Pos && !hasMoved)
                    {
                        selectUnit.Select(false); //�I������
                        if (cursors != null)
                        {
                            foreach (var item in cursors)
                            {
                                Destroy(item);
                            }
                            cursors.Clear(); //�O�̂��̂�����
                        }
                        if (attackCursors != null)
                        {
                            foreach (var item in attackCursors)
                            {
                                Destroy(item);
                            }
                            attackCursors.Clear(); //�O�̂��̂�����
                        }
                        reChoice = true; //�đI���\��
                        Movable = false; //�I����ԁA�܂�ړ��s��
                        return;
                    }
                    if (selectUnit && Movable && !hasMoved && tile && !movableTiles.ContainsKey(Vec2pos) && unit != units[Vec2pos.x, Vec2pos.y])
                    {
                        Debug.Log("�ʃ��j�b�g�I��");
                        preventDoubleSelect();
                        reChoice = false;
                        foreach (var item in tiles)
                        {
                            if (item.Value == tile)
                            {
                                unit = units[item.Key.x, item.Key.y];
                            }
                        }
                        if (unit == null)
                        {
                            selectUnit = null;
                            Debug.Log("unit==null");
                            return;
                        }
                    }
                }
            }
        }
        

        if (tile == null && unit == null) {
            return;
        }

        if (unit != null)
        {

            if ((unit.side == Side.P1 && P1turn) ||(unit.side == Side.P2 && P2turn) )
            {
                Debug.Log("Player");
                player = true;
            }
            else
            {
                Debug.Log("not player");
                player = false;
            }
            selectCursors(unit,player); //unit������΁A�I�����J�n
        }
        
    }

    public void Awaken()
    {
        Debug.Log(in1+","+in2+","+in3+","+in4+","+in5+","+in6);
        int[,] boardSetting =
       {
            {0,in1,0,0,in6,0 },
            {0,in2,0,0,in5,0 },
            {0,in3,0,0,in4,0 }
        };
        int boardWidth = boardSetting.GetLength(0);
        int boardHeight = boardSetting.GetLength(1);
        //�t�B�[���h������
        tiles = new Dictionary<Vector2Int, GameObject>();
        units = new Unitinfo[boardWidth, boardHeight];
        //�ړ��\�͈�
        movableTiles = new Dictionary<Vector2Int, GameObject>();
        attackableTiles = new Dictionary<Vector2Int, GameObject>();
        cursors = new List<GameObject>();
        attackCursors = new List<GameObject>();

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                int x = i - boardWidth / 2;
                int y = j - boardHeight / 2;

                Vector3 pos = new Vector3(10 * x, 0, 10 * y);
                Vector2Int idx = new Vector2Int(i, j);
                //�^�C���쐬
                GameObject tile = Instantiate(prefabtile, pos, Quaternion.identity);
                tile.name = string.Format("{0},{1}", i, j);
                tiles.Add(idx, tile); //(0,0, tile0)�̂悤�Ɋ��蓖��

                //�ړ��\�͈�
                movableTiles.Add(idx, tile);

                int type = boardSetting[i, j] % 10;
                int player = boardSetting[i, j] / 10;

                if (0 == type) continue;

                int Fixer = 0;//�ƐԂ̒����̂��߂̐��l
                pos.y = 11f;
                if (j <= 3)
                {
                    Fixer += -1;
                }
                else if (j >= 4)
                {
                    Fixer += 3;
                }
                GameObject prefab = prefabUnits[type + Fixer];
                GameObject unit = Instantiate(prefab, pos, Quaternion.Euler(0, player * 180 + 180, 0));
                Unitinfo unitinfo = unit.AddComponent<Unitinfo>();

                unitinfo.Init(player, type, tile, idx, this);

                units[i, j] = unitinfo;

                UpdateTileMaterial(tile, unitinfo.HP);
            }
        }
        UpdateMaterial();
    }

    void UpdateTileMaterial(GameObject tile, int HP) //�̗̓}�e���A��
    {
        Material selectedMaterial = null;

        switch (HP)
        {
            case 1: selectedMaterial = num1; break;
            case 2: selectedMaterial = num2; break;
            case 3: selectedMaterial = num3; break;
            case 4: selectedMaterial = num4; break;
            case 5: selectedMaterial = num5; break;
            case 6: selectedMaterial = num6; break;
            case 7: selectedMaterial = num7; break;
            case 8: selectedMaterial = num8; break;
            case 9: selectedMaterial = num9; break;
            default: selectedMaterial = num10; break; // �f�t�H���g�̃}�e���A��
        }

        Renderer renderer = tile.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = selectedMaterial;
        }
    }

    public void UpdateMaterial()
    {
        for (int i = 0; i < units.GetLength(0); i++)
        {
            for (int j = 0; j < units.GetLength(1); j++)
            {
                Vector2Int pos = new Vector2Int(i, j);
                GameObject tile;

                // Dictionary ���� tile ���擾
                if (tiles.TryGetValue(pos, out tile))
                {
                    if (units[i, j] != null)
                    {
                        UpdateTileMaterial(tile, units[i, j].HP); // �^�C���ƃ��j�b�g��HP�Ń}�e���A���X�V
                    }
                    else
                    {
                        tile.GetComponent<Renderer>().material = num0; // ���j�b�g�����Ȃ��ꍇ�̓}�e���A�����N���A
                    }
                }
                else
                {
                }
            }
        }
    }


    //�I����
    void selectCursors(Unitinfo unit, bool playerunit = true)
    {
        cursorReset(); //�J�[�\�����Z�b�g
        preventDoubleSelect();

        List<Vector2Int>movabletiles = GetMovableTiles(unit);//�ړ��\�^�C���擾
        List<Vector2Int>attackabletiles = GetAttackableunits(unit);//�U���\�^�C���擾
        this.movableTiles.Clear();
        this.attackableTiles.Clear();
        if(movabletiles.Count == 0 &&(P1HowManyMoved>=2 || P2HowManyMoved>=2))
        {
            Debug.Log("�o��Y");
            selectUnit.Select(false);
            compulsionStop(unit);
            Movable = false;
            return;
        }
        if (!unit) return;

        foreach (var item in movabletiles) //�ړ��\�͈͒ǉ�
        {
            if (!this.movableTiles.ContainsKey(item))
            {
                this.movableTiles.Add(item, tiles[item]);
                Vector3 pos = tiles[item].transform.position;
                pos.y += 5f;
                GameObject cursor = Instantiate(prefabCursor, pos, Quaternion.identity);
                cursors.Add(cursor);
            }
                
        }

        foreach (var item in attackabletiles)
        {
            if (!this.attackableTiles.ContainsKey(item))
            {
                this.attackableTiles.Add(item, tiles[item]);
                Vector3 pos = tiles[item].transform.position;
                pos.y += 5f;
                GameObject cursor = Instantiate(AttackCursor, pos, Quaternion.identity);
                attackCursors.Add(cursor);
            }

        }
        unit.Select(true);
        selectUnit = unit;
        if (playerunit) //�v���C���[���j�b�g�̂ݓ�����悤��
        {
            Movable = true;
        }
        else
        {
            Movable = false;
        }

        audioSource.PlayOneShot(choice);
    }

    void moveUnit(Unitinfo unit, Vector2Int tileindex)
    {
        cursorReset(); //�J�[�\�����Z�b�g

        // ���݂̈ʒu�Ɠ����ꍇ�͏������I������
        if (unit.Pos == tileindex)
        {
            if (attackCursors != null)
            {
                foreach (var item in attackCursors)
                {
                    Destroy(item);
                }
                attackCursors.Clear(); //�O�̂��̂�����
            }
            return;
        }
        audioSource.PlayOneShot(put);

        if (unit.unitMoved == false)
        {
            if(Turn%2 == 1)
            {
                P1HowManyMoved += 1;
            }
            else if (Turn%2 == 0)
            {
                P2HowManyMoved += 1;
            }
            Vector2Int oldpos = unit.Pos; //���ݒn
            unit.Move(tiles[tileindex], tileindex,this); //���j�b�g�ړ�

            if (P1HowManyMoved >= P1Num)�@//P1�̃^�[���_���[�W����
            {
                if(Turn!=1)//1�^�[���ڂ�������
                {
                    unit.Damage(units);
                }
                StartCoroutine(DelayCoroutine());
            }
            if(P2HowManyMoved >= P2Num) //P2�̃^�[���_���[�W����
            {
                unit.Damage(units);
                unit.HPShow(HPshow);
                StartCoroutine(DelayCoroutine());
            }

            if (units[tileindex.x, tileindex.y] != units[oldpos.x, oldpos.y])
            {
                units[tileindex.x, tileindex.y] = unit;
                units[oldpos.x, oldpos.y] = null;
            }
            else
            {
                units[oldpos.x, oldpos.y] = unit;
            }


            unit.fieldStatus = FieldStatus.OnBoard;
            UpdateMaterial();
        }
    }

    List<Vector2Int> GetMovableTiles(Unitinfo unit) //unit����̎擾
    {
        List<Vector2Int> ret = unit.GetMovableTiles(units);
        return ret;
    }

    List<Vector2Int> GetAttackableunits(Unitinfo unit)
    {
        List<Vector2Int> ret = unit.GetAttackableunits(units);
        return ret;
    }
    void preventDoubleSelect()
    {
        if (selectUnit != null) //�I�����j�b�g���I����Ԃɂ���
        {
            selectUnit.Select(false); //Unselect��Y�ɂ���
            selectUnit = null; //selectUnit���d���h�~�ɏ���
        }
    }
    void cursorReset()
    {
        if (cursors != null)
        {
            foreach (var item in cursors)
            {
                Destroy(item);
            }
            cursors.Clear(); //�O�̂��̂�����
        }
        if (attackCursors != null)
        {
            foreach (var item in attackCursors)
            {
                Destroy(item);
            }
            attackCursors.Clear(); //�O�̂��̂�����
        }
    }


    void ResetTurn()
    {
        audioSource.PlayOneShot(change);
        P1HowManyMoved = 0;
        P2HowManyMoved = 0;
        hasMoved = false;

        P1Num = 0; // P1�̃��j�b�g�������Z�b�g
        P2Num = 0; // P2�̃��j�b�g�������Z�b�g

        CameraMove cameramove = cameraParent.GetComponent<CameraMove>();
        cameramove.ChangeRotation();

        // �S���j�b�g�̏�Ԃ����Z�b�g
        for (int i = 0; i < units.GetLength(0); i++)
        {
            for (int j = 0; j < units.GetLength(1); j++)
            {
                if (units[i, j] != null)
                {
                    units[i, j].unitMovedReset();

                    // ���j�b�g�̃T�C�h�ɉ����ăJ�E���g�𑝂₷
                    if (units[i, j].side == Side.P1)
                    {
                        P1Num++;
                    }
                    else if (units[i, j].side == Side.P2)
                    {
                        P2Num++;
                    }
                }
                Vector2Int pos = new Vector2Int(i, j);
                GameObject tile = tiles[pos];

                if (units[i, j] != null)
                {
                    UpdateTileMaterial(tile, units[i, j].HP); // �^�C���ƃ��j�b�g��HP�Ń}�e���A���X�V
                }
                else
                {
                    tile.GetComponent<Renderer>().material = null; // ���j�b�g�����Ȃ��ꍇ�̓}�e���A�����N���A
                }
            }
        }
        if((P2Num ==0) || (P1Num == 0))
        {
            P1TurnTXT.gameObject.SetActive(false);
            P2TurnTXT.gameObject.SetActive(false);
        }
        if(P2Num == 0)
        {
            if(P1Win != null)
            {
                P1Win.gameObject.SetActive(true);
                audioSource.PlayOneShot(winning);
            }
        }
        if (P1Num == 0)
        {
            if (P2Win != null)
            {
                P2Win.gameObject.SetActive(true);
                audioSource.PlayOneShot(winning);
            }
        }
    }
    void compulsionStop(Unitinfo unit/*, Vector2Int tileindex*/)
    {
        Debug.Log("CompulsionStop");
        audioSource.PlayOneShot(put);

        if (unit.unitMoved == false)
        {
            if (Turn % 2 == 1)
            {
                P1HowManyMoved += 1;
            }
            else if (Turn % 2 == 0)
            {
                P2HowManyMoved += 1;
            }
            Vector2Int oldpos = unit.Pos; //���ݒn

            if (P1HowManyMoved >= P1Num)�@//P1�̃^�[���_���[�W����
            {
                if (Turn != 1)
                {
                    unit.Damage(units);
                }
                P2turn = true;
                P1turn = false;
                Turn += 1;
                StartCoroutine(DelayCoroutine());
            }
            if (P2HowManyMoved >= P2Num) //P2�̃^�[���_���[�W����
            {
                unit.Damage(units);
                P2turn = false;
                P1turn = true;
                Turn += 1;
                StartCoroutine(DelayCoroutine());
            }

            unit.fieldStatus = FieldStatus.OnBoard;
            UpdateMaterial();
        }
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(1);

        if (Turn %2== 1)
        {
            P2turn = true;
            P1turn = false;
            Turn += 1;
            P2TurnTXT.gameObject.SetActive(true);
            P1TurnTXT.gameObject.SetActive(false);
        }
        else if (Turn %2== 0)
        {
            P2turn = false;
            P1turn = true;
            Turn += 1;
            P1TurnTXT.gameObject.SetActive(true);
            P2TurnTXT.gameObject.SetActive(false);
        }
        ResetTurn();
        UpdateMaterial();

    }
}
