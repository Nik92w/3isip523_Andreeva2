using System;
using System.Collections.Generic;
using System.Linq;

namespace _3ИСИП523_Андреева1
{
    public abstract class Person
    {
        // ИНКАПСУЛЯЦИЯ: Поля защищены от прямого доступа
        private string _name;
        private int _age;
        private string _contactInfo;
        private static int _nextId = 1;
        private int _id;

        // контрол доступ
        public int Id
        {
            get => _id;
            private set => _id = value;
        }
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Имя не может быть пустым");
                _name = value;
            }
        }
        public int Age
        {
            get => _age;
            set
            {
                if (value < 16 || value > 100)
                    throw new ArgumentException("Возраст должен быть от 16 до 100 лет");
                _age = value;
            }
        }
        public string ContactInfo
        {
            get => _contactInfo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Контактная информация не может быть пустой");
                _contactInfo = value;
            }
        }
        // Конструктор абстрактного класса
        protected Person(string name, int age, string contactInfo)
        {
            Id = _nextId++;
            Name = name;
            Age = age;
            ContactInfo = contactInfo;
        }

        // ПОЛИМОРФИЗМ: Абстрактный метод - каждая производный класс реализует его по-своему
        public abstract string GetRole();
        // ПОЛИМОРФИЗМ: Виртуальный метод - можно переопределить в производных классах
        public virtual string GetInfo()
        {
            return $"{Name}, {Age} лет, {GetRole()}";
        }
    }
    public class Student : Person  // НАСЛЕДОВАНИЕ
    {
        // ИНКАПСУЛЯЦИЯ: Список курсов защищен от прямого изменения
        private List<Course> _courses;
        public IReadOnlyList<Course> Courses => _courses.AsReadOnly();
        public string Major { get; set; }
        public int Year { get; set; }
        public Student(string name, int age, string contactInfo, string major, int year)
            : base(name, age, contactInfo) // Вызов конструктора базового класса
        {
            Major = major;
            Year = year;
            _courses = new List<Course>();
        }
        // ПОЛИМОРФИЗМ: Реализация абстрактного метода
        public override string GetRole()
        {
            return "Студент";
        }
        // ПОЛИМОРФИЗМ: Переопределение виртуального метода
        public override string GetInfo()
        {
            return $"{base.GetInfo()}, Специальность: {Major}, {Year} курс";
        }
        // Методы для работы с курсами
        public void EnrollInCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (!_courses.Contains(course))
            {
                _courses.Add(course);
                course.AddStudent(this);
            }
        }
        public void DropCourse(Course course)
        {
            if (_courses.Contains(course))
            {
                _courses.Remove(course);
                course.RemoveStudent(this);
            }
        }
        public string GetCourseList()
        {
            if (_courses.Count == 0)
                return "Не записан на курсы";

            string result = "Курсы: ";
            foreach (var course in _courses)
            {
                result += $"{course.CourseName}, ";
            }
            return result.TrimEnd(',', ' ');
        }
    }
    public class Professor : Person
    {
        public string Department { get; set; }
        public string Specialization { get; set; }

        // ИНКАПСУЛЯЦИЯ: Список курсов защищен
        private List<Course> _coursesTeaching;
        public IReadOnlyList<Course> CoursesTeaching => _coursesTeaching.AsReadOnly();
        public Professor(string name, int age, string contactInfo, string department, string specialization)
            : base(name, age, contactInfo)
        {
            Department = department;
            Specialization = specialization;
            _coursesTeaching = new List<Course>();
        }
        // ПОЛИМОРФИЗМ: Реализация абстрактного метода
        public override string GetRole()
        {
            return "Преподаватель";
        }
        // ПОЛИМОРФИЗМ: Переопределение виртуального метода
        public override string GetInfo()
        {
            return $"{base.GetInfo()}, Кафедра: {Department}, Специализация: {Specialization}";
        }
        public void AssignToCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (!_coursesTeaching.Contains(course))
            {
                _coursesTeaching.Add(course);
                course.AssignProfessor(this);
            }
        }

        public void RemoveFromCourse(Course course)
        {
            if (_coursesTeaching.Contains(course))
            {
                _coursesTeaching.Remove(course);
                if (course.Professor == this)
                {
                    course.RemoveProfessor();
                }
            }
        }
        public string GetTeachingCourses()
        {
            if (_coursesTeaching.Count == 0)
                return "Не преподает курсы";

            string result = "Преподаваемые курсы: ";
            foreach (var course in _coursesTeaching)
            {
                result += $"{course.CourseName}, ";
            }
            return result.TrimEnd(',', ' ');
        }

        // Метод для редактирования данных преподавателя
        public void UpdateProfessor(string name, int age, string contactInfo, string department, string specialization)
        {
            Name = name;
            Age = age;
            ContactInfo = contactInfo;
            Department = department;
            Specialization = specialization;
        }
    }
    public class Course
    {
        private static int _nextCourseId = 1;

        // ИНКАПСУЛЯЦИЯ: Все поля защищены
        private string _courseName;
        private string _courseCode;
        private int _credits;
        private Professor _professor;
        private List<Student> _enrolledStudents;
        public int CourseId { get; private set; }
        public string CourseName
        {
            get => _courseName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название курса не может быть пустым");
                _courseName = value;
            }
        }
        public string CourseCode
        {
            get => _courseCode;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Код курса не может быть пустым");
                _courseCode = value;
            }
        }
        public int Credits
        {
            get => _credits;
            set
            {
                if (value < 1 || value > 10)
                    throw new ArgumentException("Кредиты должны быть от 1 до 10");
                _credits = value;
            }
        }
        public Professor Professor => _professor;
        public IReadOnlyList<Student> EnrolledStudents => _enrolledStudents.AsReadOnly();
        public Course(string courseName, string courseCode, int credits)
        {
            CourseId = _nextCourseId++;
            CourseName = courseName;
            CourseCode = courseCode;
            Credits = credits;
            _enrolledStudents = new List<Student>();
        }
        public void AssignProfessor(Professor professor)
        {
            _professor = professor;
        }

        public void RemoveProfessor()
        {
            _professor = null;
        }
        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (!_enrolledStudents.Contains(student))
            {
                _enrolledStudents.Add(student);
            }
        }
        public void RemoveStudent(Student student)
        {
            if (_enrolledStudents.Contains(student))
            {
                _enrolledStudents.Remove(student);
            }
        }

        // Метод для редактирования данных курса
        public void UpdateCourse(string courseName, string courseCode, int credits)
        {
            CourseName = courseName;
            CourseCode = courseCode;
            Credits = credits;
        }

        public string GetCourseInfo()
        {
            string professorInfo = _professor != null ? _professor.Name : "Не назначен";
            return $"Курс: {CourseName} ({CourseCode}), Преподаватель: {professorInfo}, " +
                   $"Кредиты: {Credits}, Студентов: {_enrolledStudents.Count}";
        }

        public string GetStudentList()
        {
            if (_enrolledStudents.Count == 0)
                return "На курс не записаны студенты";

            string result = "Студенты курса:\n";
            foreach (var student in _enrolledStudents)
            {
                result += $"- {student.Name} ({student.Major})\n";
            }
            return result;
        }
    }
    public class University
    {
        // ИНКАПСУЛЯЦИЯ: Все коллекции защищены от прямого доступа
        private List<Person> _people;
        private List<Course> _courses;

        public University()
        {
            _people = new List<Person>();
            _courses = new List<Course>();
            InitializeSampleData();
        }

        // Методы для работы со студентами
        public void AddStudent(string name, int age, string contactInfo, string major, int year)
        {
            var student = new Student(name, age, contactInfo, major, year);
            _people.Add(student);
        }

        // Методы для работы с преподавателями
        public void AddProfessor(string name, int age, string contactInfo, string department, string specialization)
        {
            var professor = new Professor(name, age, contactInfo, department, specialization);
            _people.Add(professor);
        }

        // Методы для работы с курсами
        public void AddCourse(string courseName, string courseCode, int credits)
        {
            var course = new Course(courseName, courseCode, credits);
            _courses.Add(course);
        }

        // ПОЛИМОРФИЗМ: Работаем с разными типами через базовый класс Person
        public List<Student> GetAllStudents()
        {
            return _people.OfType<Student>().ToList();
        }

        public List<Professor> GetAllProfessors()
        {
            return _people.OfType<Professor>().ToList();
        }

        public List<Course> GetAllCourses()
        {
            return new List<Course>(_courses);
        }

        public Student FindStudentById(int id)
        {
            return _people.OfType<Student>().FirstOrDefault(s => s.Id == id);
        }

        public Professor FindProfessorById(int id)
        {
            return _people.OfType<Professor>().FirstOrDefault(p => p.Id == id);
        }

        public Course FindCourseById(int id)
        {
            return _courses.FirstOrDefault(c => c.CourseId == id);
        }

        public void EnrollStudentInCourse(int studentId, int courseId)
        {
            var student = FindStudentById(studentId);
            var course = FindCourseById(courseId);

            if (student != null && course != null)
            {
                student.EnrollInCourse(course);
            }
        }

        public void AssignProfessorToCourse(int professorId, int courseId)
        {
            var professor = FindProfessorById(professorId);
            var course = FindCourseById(courseId);

            if (professor != null && course != null)
            {
                professor.AssignToCourse(course);
            }
        }

        // Удаление преподавателя из курса
        public void RemoveProfessorFromCourse(int professorId, int courseId)
        {
            var professor = FindProfessorById(professorId);
            var course = FindCourseById(courseId);

            if (professor != null && course != null)
            {
                professor.RemoveFromCourse(course);
            }
        }

        // Удаление студента из курса
        public void RemoveStudentFromCourse(int studentId, int courseId)
        {
            var student = FindStudentById(studentId);
            var course = FindCourseById(courseId);

            if (student != null && course != null)
            {
                student.DropCourse(course);
            }
        }

        // Удаление преподавателя
        public bool RemoveProfessor(int professorId)
        {
            var professor = FindProfessorById(professorId);
            if (professor != null)
            {
                // Удаляем преподавателя из всех курсов
                foreach (var course in _courses.Where(c => c.Professor == professor).ToList())
                {
                    course.RemoveProfessor();
                }
                return _people.Remove(professor);
            }
            return false;
        }

        // Удаление курса
        public bool RemoveCourse(int courseId)
        {
            var course = FindCourseById(courseId);
            if (course != null)
            {
                // Удаляем курс у всех студентов
                foreach (var student in _people.OfType<Student>())
                {
                    student.DropCourse(course);
                }
                return _courses.Remove(course);
            }
            return false;
        }

        // Инициализация тестовыми данными
        private void InitializeSampleData()
        {
            // Добавляем преподавателей
            AddProfessor("Иван Петров", 45, "ivan@university.ru", "Компьютерные науки", "Программирование", 80000);
            AddProfessor("Мария Сидорова", 38, "maria@university.ru", "Математика", "Алгебра", 75000);
            AddProfessor("Алексей Козлов", 50, "alex@university.ru", "Физика", "Квантовая механика", 90000);

            // Добавляем студентов
            AddStudent("Анна Иванова", 20, "anna@student.ru", "Компьютерные науки", 2);
            AddStudent("Дмитрий Смирнов", 21, "dmitry@student.ru", "Математика", 3);
            AddStudent("Елена Кузнецова", 19, "elena@student.ru", "Физика", 1);
            AddStudent("Сергей Попов", 22, "sergey@student.ru", "Компьютерные науки", 4);
            AddStudent("Ольга Васильева", 20, "olga@student.ru", "Математика", 2);

            // Добавляем курсы
            AddCourse("Программирование на C#", "CS101", 5);
            AddCourse("Алгебра и геометрия", "MATH201", 6);
            AddCourse("Квантовая физика", "PHYS301", 7);
            AddCourse("Базы данных", "CS202", 4);
            AddCourse("Математический анализ", "MATH102", 6);

            // Назначаем преподавателей на курсы
            AssignProfessorToCourse(1, 1); // Иван Петров на Программирование
            AssignProfessorToCourse(2, 2); // Мария Сидорова на Алгебру
            AssignProfessorToCourse(3, 3); // Алексей Козлов на Квантовую физику
            AssignProfessorToCourse(1, 4); // Иван Петров на Базы данных
            AssignProfessorToCourse(2, 5); // Мария Сидорова на Матанализ

            // Записываем студентов на курсы
            EnrollStudentInCourse(1, 1); // Анна на Программирование
            EnrollStudentInCourse(1, 4); // Анна на Базы данных
            EnrollStudentInCourse(2, 2); // Дмитрий на Алгебру
            EnrollStudentInCourse(2, 5); // Дмитрий на Матанализ
            EnrollStudentInCourse(3, 3); // Елена на Квантовую физику
            EnrollStudentInCourse(4, 1); // Сергей на Программирование
            EnrollStudentInCourse(5, 2); // Ольга на Алгебру
        }
    }
    class Program
    {
        private static University _university = new University();

        static void Main(string[] args)
        {
            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ ===");

            bool running = true;
            while (running)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageStudents();
                        break;
                    case "2":
                        ManageProfessors();
                        break;
                    case "3":
                        ManageCourses();
                        break;
                    case "4":
                        DisplayAllInformation();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Выход из системы...");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("\nГлавное меню:");
            Console.WriteLine("1 - Управление студентами");
            Console.WriteLine("2 - Управление преподавателями");
            Console.WriteLine("3 - Управление курсами");
            Console.WriteLine("4 - Просмотр всей информации");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");
        }

        static void ManageStudents()
        {
            bool inStudentMenu = true;
            while (inStudentMenu)
            {
                Console.WriteLine("\n--- Управление студентами ---");
                Console.WriteLine("1 - Добавить студента");
                Console.WriteLine("2 - Просмотреть всех студентов");
                Console.WriteLine("3 - Найти студента по ID");
                Console.WriteLine("4 - Записать студента на курс");
                Console.WriteLine("5 - Вернуться в главное меню");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddNewStudent();
                        break;
                    case "2":
                        DisplayAllStudents();
                        break;
                    case "3":
                        FindStudent();
                        break;
                    case "4":
                        EnrollStudent();
                        break;
                    case "5":
                        inStudentMenu = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static void AddNewStudent()
        {
            try
            {
                Console.Write("Введите имя: ");
                string name = Console.ReadLine();

                Console.Write("Введите возраст: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Введите контактную информацию: ");
                string contact = Console.ReadLine();

                Console.Write("Введите специальность: ");
                string major = Console.ReadLine();

                Console.Write("Введите курс: ");
                int year = int.Parse(Console.ReadLine());

                _university.AddStudent(name, age, contact, major, year);
                Console.WriteLine("Студент успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void DisplayAllStudents()
        {
            var students = _university.GetAllStudents();
            if (students.Count == 0)
            {
                Console.WriteLine("Студенты не найдены.");
                return;
            }

            Console.WriteLine("\n--- Все студенты ---");
            foreach (var student in students)
            {
                // ПОЛИМОРФИЗМ: Вызывается переопределенный метод GetInfo()
                Console.WriteLine($"ID: {student.Id}, {student.GetInfo()}");
                Console.WriteLine($"  {student.GetCourseList()}");
                Console.WriteLine();
            }
        }

        static void FindStudent()
        {
            Console.Write("Введите ID студента: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var student = _university.FindStudentById(id);
                if (student != null)
                {
                    Console.WriteLine("\n--- Найден студент ---");
                    Console.WriteLine(student.GetInfo());
                    Console.WriteLine(student.GetCourseList());
                }
                else
                {
                    Console.WriteLine("Студент не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ID.");
            }
        }

        static void EnrollStudent()
        {
            Console.Write("Введите ID студента: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Неверный формат ID студента.");
                return;
            }

            Console.Write("Введите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Неверный формат ID курса.");
                return;
            }

            _university.EnrollStudentInCourse(studentId, courseId);
            Console.WriteLine("Студент записан на курс!");
        }

        static void ManageProfessors()
        {
            bool inProfessorMenu = true;
            while (inProfessorMenu)
            {
                Console.WriteLine("\n--- Управление преподавателями ---");
                Console.WriteLine("1 - Добавить преподавателя");
                Console.WriteLine("2 - Просмотреть всех преподавателей");
                Console.WriteLine("3 - Найти преподавателя по ID");
                Console.WriteLine("4 - Редактировать преподавателя");
                Console.WriteLine("5 - Назначить преподавателя на курс");
                Console.WriteLine("6 - Удалить преподавателя из курса");
                Console.WriteLine("7 - Удалить преподавателя");
                Console.WriteLine("8 - Вернуться в главное меню");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddNewProfessor();
                        break;
                    case "2":
                        DisplayAllProfessors();
                        break;
                    case "3":
                        FindProfessor();
                        break;
                    case "4":
                        EditProfessor();
                        break;
                    case "5":
                        AssignProfessorToCourse();
                        break;
                    case "6":
                        RemoveProfessorFromCourse();
                        break;
                    case "7":
                        RemoveProfessor();
                        break;
                    case "8":
                        inProfessorMenu = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static void AddNewProfessor()
        {
            try
            {
                Console.Write("Введите имя: ");
                string name = Console.ReadLine();

                Console.Write("Введите возраст: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Введите контактную информацию: ");
                string contact = Console.ReadLine();

                Console.Write("Введите кафедру: ");
                string department = Console.ReadLine();

                Console.Write("Введите специализацию: ");
                string specialization = Console.ReadLine();

                _university.AddProfessor(name, age, contact, department, specialization);
                Console.WriteLine("Преподаватель успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void DisplayAllProfessors()
        {
            var professors = _university.GetAllProfessors();
            if (professors.Count == 0)
            {
                Console.WriteLine("Преподаватели не найдены.");
                return;
            }

            Console.WriteLine("\n--- Все преподаватели ---");
            foreach (var professor in professors)
            {
                Console.WriteLine($"ID: {professor.Id}, {professor.GetInfo()}" );
                Console.WriteLine($"  {professor.GetTeachingCourses()}");
                Console.WriteLine();
            }
        }

        static void FindProfessor()
        {
            Console.Write("Введите ID преподавателя: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var professor = _university.FindProfessorById(id);
                if (professor != null)
                {
                    Console.WriteLine("\n--- Найден преподаватель ---");
                    Console.WriteLine(professor.GetInfo());
                    Console.WriteLine(professor.GetTeachingCourses());
                }
                else
                {
                    Console.WriteLine("Преподаватель не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ID.");
            }
        }

        static void EditProfessor()
        {
            Console.Write("Введите ID преподавателя для редактирования: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID.");
                return;
            }

            var professor = _university.FindProfessorById(id);
            if (professor == null)
            {
                Console.WriteLine("Преподаватель не найден.");
                return;
            }

            try
            {
                Console.WriteLine($"Текущие данные: {professor.GetInfo()}");

                Console.Write("Введите новое имя (оставьте пустым для сохранения текущего): ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name)) name = professor.Name;

                Console.Write("Введите новый возраст (0 для сохранения текущего): ");
                string ageInput = Console.ReadLine();
                int age = string.IsNullOrWhiteSpace(ageInput) ? professor.Age : int.Parse(ageInput);

                Console.Write("Введите новые контакты (оставьте пустым для сохранения текущих): ");
                string contact = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(contact)) contact = professor.ContactInfo;

                Console.Write("Введите новую кафедру (оставьте пустым для сохранения текущей): ");
                string department = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(department)) department = professor.Department;

                Console.Write("Введите новую специализацию (оставьте пустым для сохранения текущей): ");
                string specialization = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(specialization)) specialization = professor.Specialization;

                professor.UpdateProfessor(name, age, contact, department, specialization);
                Console.WriteLine("Данные преподавателя успешно обновлены!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при редактировании: {ex.Message}");
            }
        }

        static void AssignProfessorToCourse()
        {
            Console.Write("Введите ID преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int professorId))
            {
                Console.WriteLine("Неверный формат ID преподавателя.");
                return;
            }

            Console.Write("Введите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Неверный формат ID курса.");
                return;
            }

            _university.AssignProfessorToCourse(professorId, courseId);
            Console.WriteLine("Преподаватель назначен на курс!");
        }

        static void RemoveProfessorFromCourse()
        {
            Console.Write("Введите ID преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int professorId))
            {
                Console.WriteLine("Неверный формат ID преподавателя.");
                return;
            }

            Console.Write("Введите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Неверный формат ID курса.");
                return;
            }

            _university.RemoveProfessorFromCourse(professorId, courseId);
            Console.WriteLine("Преподаватель удален с курса!");
        }

        static void RemoveProfessor()
        {
            Console.Write("Введите ID преподавателя для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int professorId))
            {
                Console.WriteLine("Неверный формат ID.");
                return;
            }

            var professor = _university.FindProfessorById(professorId);
            if (professor == null)
            {
                Console.WriteLine("Преподаватель не найден.");
                return;
            }

            Console.Write($"Вы уверены, что хотите удалить преподавателя {professor.Name}? (y/n): ");
            string confirmation = Console.ReadLine().ToLower();

            if (confirmation == "y" || confirmation == "д")
            {
                if (_university.RemoveProfessor(professorId))
                {
                    Console.WriteLine("Преподаватель успешно удален!");
                }
                else
                {
                    Console.WriteLine("Ошибка при удалении преподавателя.");
                }
            }
            else
            {
                Console.WriteLine("Удаление отменено.");
            }
        }

        static void ManageCourses()
        {
            bool inCourseMenu = true;
            while (inCourseMenu)
            {
                Console.WriteLine("\n--- Управление курсами ---");
                Console.WriteLine("1 - Добавить курс");
                Console.WriteLine("2 - Просмотреть все курсы");
                Console.WriteLine("3 - Найти курс по ID");
                Console.WriteLine("4 - Редактировать курс");
                Console.WriteLine("5 - Просмотреть студентов курса");
                Console.WriteLine("6 - Удалить студента из курса");
                Console.WriteLine("7 - Удалить курс");
                Console.WriteLine("8 - Вернуться в главное меню");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddNewCourse();
                        break;
                    case "2":
                        DisplayAllCourses();
                        break;
                    case "3":
                        FindCourse();
                        break;
                    case "4":
                        EditCourse();
                        break;
                    case "5":
                        DisplayCourseStudents();
                        break;
                    case "6":
                        RemoveStudentFromCourse();
                        break;
                    case "7":
                        RemoveCourse();
                        break;
                    case "8":
                        inCourseMenu = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static void AddNewCourse()
        {
            try
            {
                Console.Write("Введите название курса: ");
                string name = Console.ReadLine();

                Console.Write("Введите код курса: ");
                string code = Console.ReadLine();

                Console.Write("Введите количество кредитов: ");
                int credits = int.Parse(Console.ReadLine());

                _university.AddCourse(name, code, credits);
                Console.WriteLine("Курс успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void DisplayAllCourses()
        {
            var courses = _university.GetAllCourses();
            if (courses.Count == 0)
            {
                Console.WriteLine("Курсы не найдены.");
                return;
            }

            Console.WriteLine("\n--- Все курсы ---");
            foreach (var course in courses)
            {
                Console.WriteLine($"ID: {course.CourseId}, {course.GetCourseInfo()}");
                Console.WriteLine();
            }
        }

        static void FindCourse()
        {
            Console.Write("Введите ID курса: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var course = _university.FindCourseById(id);
                if (course != null)
                {
                    Console.WriteLine("\n--- Найден курс ---");
                    Console.WriteLine(course.GetCourseInfo());
                    Console.WriteLine(course.GetStudentList());
                }
                else
                {
                    Console.WriteLine("Курс не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ID.");
            }
        }

        static void EditCourse()
        {
            Console.Write("Введите ID курса для редактирования: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат ID.");
                return;
            }

            var course = _university.FindCourseById(id);
            if (course == null)
            {
                Console.WriteLine("Курс не найден.");
                return;
            }

            try
            {
                Console.WriteLine($"Текущие данные: {course.GetCourseInfo()}");

                Console.Write("Введите новое название курса (оставьте пустым для сохранения текущего): ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name)) name = course.CourseName;

                Console.Write("Введите новый код курса (оставьте пустым для сохранения текущего): ");
                string code = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(code)) code = course.CourseCode;

                Console.Write("Введите новое количество кредитов (0 для сохранения текущего): ");
                string creditsInput = Console.ReadLine();
                int credits = string.IsNullOrWhiteSpace(creditsInput) ? course.Credits : int.Parse(creditsInput);

                course.UpdateCourse(name, code, credits);
                Console.WriteLine("Данные курса успешно обновлены!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при редактировании: {ex.Message}");
            }
        }

        static void DisplayCourseStudents()
        {
            Console.Write("Введите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Неверный формат ID курса.");
                return;
            }

            var course = _university.FindCourseById(courseId);
            if (course != null)
            {
                Console.WriteLine($"\n--- Студенты курса {course.CourseName} ---");
                Console.WriteLine(course.GetStudentList());
            }
            else
            {
                Console.WriteLine("Курс не найден.");
            }
        }

        static void RemoveStudentFromCourse()
        {
            Console.Write("Введите ID студента: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Неверный формат ID студента.");
                return;
            }

            Console.Write("Введите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Неверный формат ID курса.");
                return;
            }

            _university.RemoveStudentFromCourse(studentId, courseId);
            Console.WriteLine("Студент удален с курса!");
        }

        static void RemoveCourse()
        {
            Console.Write("Введите ID курса для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Неверный формат ID.");
                return;
            }

            var course = _university.FindCourseById(courseId);
            if (course == null)
            {
                Console.WriteLine("Курс не найден.");
                return;
            }

            Console.Write($"Вы уверены, что хотите удалить курс {course.CourseName}? (y/n): ");
            string confirmation = Console.ReadLine().ToLower();

            if (confirmation == "y" || confirmation == "д")
            {
                if (_university.RemoveCourse(courseId))
                {
                    Console.WriteLine("Курс успешно удален!");
                }
                else
                {
                    Console.WriteLine("Ошибка при удалении курса.");
                }
            }
            else
            {
                Console.WriteLine("Удаление отменено.");
            }
        }

        static void DisplayAllInformation()
        {
            Console.WriteLine("\n=== ПОЛНАЯ ИНФОРМАЦИЯ ОБ УНИВЕРСИТЕТЕ ===");

            Console.WriteLine("\n--- ПРЕПОДАВАТЕЛИ ---");
            var professors = _university.GetAllProfessors();
            foreach (var professor in professors)
            {
                Console.WriteLine($"ID: {professor.Id}, {professor.GetInfo()}");
                Console.WriteLine($"  {professor.GetTeachingCourses()}");
            }

            Console.WriteLine("\n--- СТУДЕНТЫ ---");
            var students = _university.GetAllStudents();
            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.Id}, {student.GetInfo()}");
            }

            Console.WriteLine("\n--- КУРСЫ ---");
            var courses = _university.GetAllCourses();
            foreach (var course in courses)
            {
                Console.WriteLine($"ID: {course.CourseId}, {course.GetCourseInfo()}");
            }
        }
    }
}