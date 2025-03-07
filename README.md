# TestTask_I-Teco

Спроектировать кадровую систему:
 1. Учет ФИО сотрудников
 2. Учет подразделений в которых числятся сотрудники.
Ограничения:
 1. Сотрудники принимаются и увольняются. Причины увольнения не интересны.
 2. Сотрудники могут переводится из подразделения в подразделение
 3. Состав подразделений может меняться в любой день. Необходимо иметь возможность получить информацию о структуре компании в любой момент времени
 4. Подразделения находятся в иерархической зависимости друг от друга.
 5. Контроль целостности данных вне задачи.
 6. Реализовать UI позволяющий 
 6.1 добавить в любой подразделение нового сотрудника
 6.2 вывести список всех подразделений на конкретную дату
 6.3 вывести сотрудников выбранного подразделения за период (у периода задается дата начала, дата окончания, выводятся все сотрудники, которые числились в подразделении хотя бы один день относящийся к заданному периоду)

Язык реализации: C#, .NET 8.0
