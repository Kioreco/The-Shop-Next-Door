using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HireApp : MonoBehaviour
{
    [Header("Random Variables")]
    private int randomGenre = 0;
    private string randomName = "";
    private float randomCategory = 0.0f;
    private int randomSalary = 0;
    private int randomPriceToHire = 0;

    private string[] maleNames = new string[] 
        { "Jess", "Dean", "Luke", "Paul", "Josh", "Patrick", "Tyberius", "Jace", "Daniel", "Alec", "William", "Chris", "Raúl", "Pablo", "Néstor", "Fran", "Edward", "Oliver", "Jacob", "Sky", "Helia", "Brandon", "Riven", "Justin", "Martí", "Kit", "Chuck", "Damon", "Stefan", "Stiles" };
    private string[] femaleNames = new string[] 
        { "Carmen", "Isabel", "Milagros", "Paula", "Rory", "Lorelai", "Laura", "Emily", "Paris", "Paqui", "Clarissa", "Remedios", "Elvira", "Phoebe", "Taylor", "Emma", "Rosaline", "Bella", "Flora", "Bloom", "Stella", "Musa", "Layla", "Aisha", "Tecna", "Silvi", "Akiko", "Heather", "Selene", "Serena", "Blair" };

    [Header("Workers To Hire -------------------")]
    [SerializeField] private TextMeshProUGUI employee_TH_name;
    [SerializeField] private TextMeshProUGUI employee_TH_salary;
    [SerializeField] private TextMeshProUGUI employee_TH_priceToHire;
    [SerializeField] private Image employee_TH_image;
    [SerializeField] private Button hireButton;

    [SerializeField] private Sprite employee_Male;
    [SerializeField] private Sprite employee_Female;

    private WorkersInfo[] workersToHire;

    [Header("Workers Hired ---------------------")]
    [Header("Male Buttons")]
    [SerializeField] private GameObject[] employee_male_buttons;
    [Header("Female Buttons")]
    [SerializeField] private GameObject[] employee_female_buttons;
    [Header("Male Salaries")]
    [SerializeField] private TextMeshProUGUI[] employee_M_salaries;
    [Header("Female Buttons")]
    [SerializeField] private TextMeshProUGUI[] employee_F_salaries;


    private int hiredWorkers = 0;
    private WorkersInfo[] workersHired = new WorkersInfo[3];

    private void Awake()
    {
        GenerateNewWorkers();
    }

    public void GenerateNewWorkers()
    {
        workersToHire = new WorkersInfo[5];

        for (int i = 0; i < 5; i++)
        {
            randomGenre = Random.Range(0, 2);

            if (randomGenre == 0) //MALE WORKER
            {
                randomName = maleNames[Random.Range(0, maleNames.Length)];
            }
            else // FEMALE WORKER
            {
                randomName = femaleNames[Random.Range(0, femaleNames.Length)];
            }

            randomCategory = Random.Range(0.0f, 100.0f);

            if (randomCategory <= 50.0f)
            {
                randomSalary = Random.Range(250, 350);
                randomPriceToHire = Random.Range(250, 350);
            }
            else if (randomCategory > 50.0f && randomCategory <= 80.0f)
            {
                randomSalary = Random.Range(180, 240);
                randomPriceToHire = Random.Range(350, 450);
            }
            else
            {
                randomSalary = Random.Range(100, 150);
                randomPriceToHire = Random.Range(450, 500);
            }


            WorkersInfo worker = new WorkersInfo(randomGenre, randomName, randomSalary, randomPriceToHire);
            workersToHire[i] = worker;
        }
        employee_TH_name.SetText(workersToHire[0].name);
        employee_TH_salary.SetText(workersToHire[0].salary.ToString() + " € / day");
        employee_TH_priceToHire.SetText(workersToHire[0].priceToHire.ToString() + " €");

        if (workersToHire[0].genre == 0) { employee_TH_image.sprite = employee_Male; }
        else if (workersToHire[0].genre == 1) { employee_TH_image.sprite = employee_Female; }

    }

    private int currentWorkerToHire = 0;

    public void ChangeWorkerToHire(int moveNumber)
    {

        if (currentWorkerToHire + moveNumber < 0)
        {
            currentWorkerToHire = 4;
        }
        else if (currentWorkerToHire + moveNumber >= workersToHire.Length)
        {
            currentWorkerToHire = 0;
        }
        else { currentWorkerToHire += moveNumber; }

        if (workersToHire[currentWorkerToHire].genre == 0) { employee_TH_image.sprite = employee_Male; }
        else if (workersToHire[currentWorkerToHire].genre == 1) { employee_TH_image.sprite = employee_Female; }

        employee_TH_name.SetText(workersToHire[currentWorkerToHire].name);
        employee_TH_salary.SetText(workersToHire[currentWorkerToHire].salary.ToString() + " € / day");
        employee_TH_priceToHire.SetText(workersToHire[currentWorkerToHire].priceToHire.ToString() + " €");

        if (!workersToHire[currentWorkerToHire].hired) { hireButton.interactable = true; }
        else {  hireButton.interactable = false; }
    }

    public void HireWorker()
    {
        if (GameManager.Instance.dineroJugador - workersToHire[currentWorkerToHire].priceToHire <= 0 || hiredWorkers == 1) { return; }

        int workersIndex = 0;

        if (workersHired[0] == null)
        {
            workersIndex = 0;
        }
        //else if (workersHired[1] == null)
        //{
        //    workersIndex = 1;
        //}
        //else if (workersHired[2] == null)
        //{
        //    workersIndex = 2;
        //}

        workersHired[workersIndex] = workersToHire[currentWorkerToHire];

        if (workersHired[workersIndex].genre == 0)
        {
            employee_M_salaries[workersIndex].SetText(workersHired[workersIndex].name);
            employee_M_salaries[workersIndex].SetText(workersHired[workersIndex].salary + "€");
            employee_male_buttons[workersIndex].SetActive(true);

            GameManager.Instance.dineroJugador -= workersHired[workersIndex].priceToHire;
            UIManager.Instance.UpdatePlayerMoney_UI();

            //Instanciar trabajador chico
        }
        else
        {
            employee_F_salaries[workersIndex].SetText(workersHired[workersIndex].name);
            employee_F_salaries[workersIndex].SetText(workersHired[workersIndex].salary + "€");
            employee_female_buttons[workersIndex].SetActive(true);

            GameManager.Instance.dineroJugador -= workersHired[workersIndex].priceToHire;
            UIManager.Instance.UpdatePlayerMoney_UI();

            //Instanciar trabajador chica
        }

        hireButton.interactable = false;
        workersHired[workersIndex].hired = true;
        workersToHire[currentWorkerToHire].hired = true;

        hiredWorkers++;
    }

    public void FireWorker(int workerNumber)
    {
        workersHired[workerNumber] = null;

        employee_male_buttons[workerNumber].SetActive(false);
        employee_female_buttons[workerNumber].SetActive(false);

        hiredWorkers--;
    }

    public void ChargeWorkerPrice()
    {
        if (workersHired[0] != null) { GameManager.Instance.dineroJugador -= workersHired[0].salary; }
    }


    //public void HireWorker()
    //{
    //    if (GameManager.Instance.dineroJugador - workersToHire[currentWorkerToHire].priceToHire <= 0 || (hiredWorkers + 1) == 4) { return; }

    //    int workersIndex = 0;

    //    if (workersHired[0] == null)
    //    {
    //        workersIndex = 0;
    //    }
    //    else if (workersHired[1] == null)
    //    {
    //        workersIndex = 1;
    //    }
    //    else if (workersHired[2] == null)
    //    {
    //        workersIndex = 2;
    //    }

    //    workersHired[workersIndex] = workersToHire[currentWorkerToHire];

    //    if (workersHired[workersIndex].genre == 0)
    //    {
    //        employee_M_salaries[workersIndex].SetText(workersHired[workersIndex].name);
    //        employee_M_salaries[workersIndex].SetText(workersHired[workersIndex].salary + "€");
    //        employee_male_buttons[workersIndex].SetActive(true);

    //        GameManager.Instance.dineroJugador -= workersHired[workersIndex].priceToHire;
    //        UIManager.Instance.UpdatePlayerMoney_UI();

    //        //Instanciar trabajador chico
    //    }
    //    else
    //    {
    //        employee_F_salaries[workersIndex].SetText(workersHired[workersIndex].name);
    //        employee_F_salaries[workersIndex].SetText(workersHired[workersIndex].salary + "€");
    //        employee_female_buttons[workersIndex].SetActive(true);

    //        GameManager.Instance.dineroJugador -= workersHired[workersIndex].priceToHire;
    //        UIManager.Instance.UpdatePlayerMoney_UI();

    //        //Instanciar trabajador chica
    //    }

    //    hireButton.interactable = false;
    //    workersHired[workersIndex].hired = true;
    //    workersToHire[currentWorkerToHire].hired = true;

    //    hiredWorkers++;
    //}

    //public void FireWorker(int workerNumber)
    //{
    //    workersHired[workerNumber] = null;

    //    employee_male_buttons[workerNumber].SetActive(false);
    //    employee_female_buttons[workerNumber].SetActive(false);

    //    hiredWorkers--;
    //}

}
