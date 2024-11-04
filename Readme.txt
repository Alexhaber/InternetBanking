Commit [2/11/2024 4:20 Am]

	No se porque me cai dormido, pero por eso tuve que empezar a codificar de madrugada, en fin.

	Se agrego un ReadMe file.

	Se agregaron 2 Dtos, aunque por ahora solo se usa el 'DepositResponse'

	Se creo el TransactionRepository
	
		@Se creo el metodo MakeCashAdvance en dicho repositorio

		@Tambien el Metodo InterAccountTransaction, pero sin configurar 

	Se hizo la interface de repositorio correspondiente, que tendre que actualizar cuando ternmine el InterAccount

	Se cambio la entidad Transacction porque esta esperaba que la id de los productos fueran ints, cuando dichas ids son strings


Commit [2/11/2024 6:22 Am]

	Se agrego TransactionService y su intefaz, se agrego su configuracion al service extension

	Solo faltarian las vistas y arreglar errores que se presenten

Commit [4/11/2024 7:35 Am]

Se me olvido subirlo ayer, bueno...

	Se agrego el PaymentController y los metodo CashAdvance y AccountToAccount yse verifico que funcionan correctamente
	, están sujetos a un rediseño de la interfaz

	AccountToAccount, no tiene link en la barra de navegacion porque no entendi completamente en que elemento iva

