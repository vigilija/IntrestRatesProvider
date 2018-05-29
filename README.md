# IntrestRatesProvider
This web application evaluates how the change of base rate will impact the interest
rate. User submit person id and new chosen base rate code. Application calculates current and new
interest rate based on specified base rate by using following formula:

interest rate = [ base rate value ] + [ margin ]

As a result it displays information about person, agreement, current base rate, interest rate for new base rate and difference.
Information is stored in spLit DB.


To launch application path to DB file need to be changed.
On Controller change variable filename with new path to web.db file. Web.db file is included in project with test data:


Goras Trusevičius, personal id 67812203006  

Amount | Base rate code | Margin | Agreement duration
:-------: |:-------------:|:----: |:------------------:
12000 | VILIBOR3m | 1.6 | 60
			

Dange Kulkavičiutė personal id 78706151287

 Amount| Base rate code| Margin| Agreement duration
:-------: |:-------------:|:----: |:------------------:
8000	  |VILIBOR1y      |2.2    |	36                
1000	  |VILIBOR6m      |1.85   |	24                
