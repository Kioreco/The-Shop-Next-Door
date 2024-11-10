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
    //[Header("Sprites")]
    //[SerializeField] private Sprite employee_M_normalSprite;
    //[SerializeField] private Sprite employee_M_hoverSprite;

    //[SerializeField] private Sprite employee_F_normalSprite;
    //[SerializeField] private Sprite employee_F_hoverSprite;

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
        //employee_TH_image.sprite = ;
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

        employee_TH_name.SetText(workersToHire[currentWorkerToHire].name);
        employee_TH_salary.SetText(workersToHire[currentWorkerToHire].salary.ToString() + " € / day");
        employee_TH_priceToHire.SetText(workersToHire[currentWorkerToHire].priceToHire.ToString() + " €");

        if (!workersToHire[currentWorkerToHire].hired) { hireButton.interactable = true; }
        else {  hireButton.interactable = false; }
    }

    public void HireWorker()
    {
        if (GameManager.Instance.dineroJugador - workersToHire[currentWorkerToHire].priceToHire <= 0 || (hiredWorkers + 1) == 4) { return; }

        print("WorkersHired lenght: " + workersHired.Length);
        print("WorkersHired value antes: " + workersHired);
        print("WorkersHired value antes: " + workersHired[hiredWorkers]);

        workersHired[hiredWorkers] = workersToHire[currentWorkerToHire];
        print("WorkersHired value despues: " + workersHired[hiredWorkers]);

        if (workersHired[hiredWorkers].genre == 0)
        {
            employee_M_salaries[hiredWorkers].SetText(workersHired[hiredWorkers].name);
            employee_M_salaries[hiredWorkers].SetText(workersHired[hiredWorkers].salary + "€");
            employee_male_buttons[hiredWorkers].SetActive(true);

            GameManager.Instance.dineroJugador -= workersHired[hiredWorkers].priceToHire;
            UIManager.Instance.UpdatePlayersMoney_UI();

            //Instanciar trabajador chico
        }
        else
        {
            employee_F_salaries[hiredWorkers].SetText(workersHired[hiredWorkers].name);
            employee_F_salaries[hiredWorkers].SetText(workersHired[hiredWorkers].salary + "€");
            employee_female_buttons[hiredWorkers].SetActive(true);

            GameManager.Instance.dineroJugador -= workersHired[hiredWorkers].priceToHire;
            UIManager.Instance.UpdatePlayersMoney_UI();

            //Instanciar trabajador chica
        }

        hireButton.interactable = false;
        workersHired[hiredWorkers].hired = true;
        workersToHire[currentWorkerToHire].hired = true;

        hiredWorkers++;
    }

    public void FireWorker(int workerNumber)
    {
        employee_male_buttons[workerNumber].SetActive(false);
        employee_female_buttons[workerNumber].SetActive(false);
        hiredWorkers--;
    }

}
