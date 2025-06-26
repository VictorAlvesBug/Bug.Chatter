var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/*
##### Functional Requirements #####

- Ao abrir o aplicativo o usu�rio pode se registrar ou entrar com uma conta existente
- Ao se registrar, deve ser informado nome e n�mero de celular ainda n�o registrado (n�mero n�o pode existir ainda no banco de dados)
- Ao entrar com uma conta, deve ser informado apenas o n�mero de celular (precisa existir no banco de dados)
- Ao informar o celular (para se registrar, ou entrar com uma conta) o usu�rio deve receber um c�digo de verifica��o por SMS
- Enquanto se registra, ao informar o c�digo correto, o usu�rio deve ser efetivamente cadastrado e ser redirecionado para a tela de conversas
- Enquanto entra na conta, ao informar o c�digo correto, o usu�rio deve ser direcionado para a tela de conversas
- Ao buscar por um n�mero de celular, deve ser retornado o usu�rio correspondente, caso exista
- Ao buscar por um nome de contato, devem ser retornados apenas contatos (com conversa)
//- Ao criar um grupo, o usu�rio deve selecionar o nome do grupo e uma lista de usu�rios (contatos novos ou j� existentes) para serem adicionados ao grupo
//- Ao criar um grupo, o usu�rio pode buscar por contatos j� existentes (por nome ou n�mero) ou por contatos novos (por n�mero)
//- Ao criar um grupo, o usu�rio que criou � adicionado como administrador do grupo
//- Em grupos, apenas administradores conseguem adicionar novos participantes, por nome (contatos existentes) ou n�mero (qualquer usu�rio)
//- Em grupos, apenas administradores conseguem remover um participante do grupo
//- Em grupos, qualquer participante pode sair do grupo
- Em uma conversa direta, ao enviar a primeira mensagem, deve ser criado um registro de conversa direta entre ambos os usu�rios, caso ainda n�o exista
//- Ao escrever uma mensagem, ela deve ser salva com status Typing (digitando)
- Ao tentar enviar uma mensagem enquanto estiver sem internet, armazen�-la num cache
- Ao reestabelecer a conex�o � internet, enviar mensagens em cache
- Ao enviar uma mensagem, ela deve ser salva com status Sent (enviada) // e com status de visualiza��o Sent (enviada)
- Ao enviar uma mensagem, todos os usu�rios participantes devem ser notificados, exceto quem enviou a mensagem
//- Ao ser recebida por todos os usu�rios, uma mensagem deve ter seu status de visualiza��o marcado como Delivered (entregue)
//- Ao ser lida por todos os usu�rios, uma mensagem deve ter seu status de visualiza��o marcado como Read (lida)



##### Use Cases #####

--> Code
Result(resultStatus, reason) SendCode(string phoneNumber)
Result(resultStatus, reason) CompareCode(string phoneNumber, string code)


--> User
Result(resultStatus, reason) ValidateNewUser(string name, string phoneNumber)
Result(resultStatus, reason) ValidateExistingUser(string phoneNumber)
Result(resultStatus, reason, data: User) RegisterUser(string name, string phoneNumber)
Result(resultStatus, reason, data: User) LoginUser(string phoneNumber)
Result(resultStatus, reason, data: User?) GetUserByNumber(string senderUserId, string phoneNumber)
Result(resultStatus, reason, data: User[]) SearchKnownUsersByName(string senderUserId, string nameQuery)
Result(resultStatus, reason) BlockUser(string senderUserId, string userIdToBlock)
//Result(resultStatus, reason, data: User) UpdateUserName(string senderUserId, string name)


--> User & Chat
Result(resultStatus, reason, data: Chat) GetChat(string senderUserId, string chatId)
Result(resultStatus, reason, data: Chat) CreateDirectChat(string senderUserId, string[] userIds)
//Result(resultStatus, reason, data: Chat) CreateGroupChat(string senderUserId, string name, string[] userIds)
Result(resultStatus, reason, data: Chat[]) ListChats(string senderUserId)
Result(resultStatus, reason, data: User[]) ListChatUsers(string senderUserId, string chatId)
Result(resultStatus, reason) AddUserToChat(string senderUserId, string chatId, string userIdToAdd)
Result(resultStatus, reason) RemoveUserFromChat(string senderUserId, string chatId, string userIdToRemove)
Result(resultStatus, reason) DeleteChat(string senderUserId, string chatId)
//Result(resultStatus, reason, data: Chat[]) SearchChat(string senderUserId, string chatQuery)


--> Chat & Message
Result(resultStatus, reason, data: Message[]) FetchMessages(string senderUserId, string chatId, long startTimestamp, long endTimestamp);


--> User & Chat & Message
Result(resultStatus, reason) SendMessage(string senderUserId, string chatId, string content)
Result(resultStatus, reason) NotifyNewMessage(string userId, string messageId, string chatId)


--> User & Message
//Result(resultStatus, reason) UpdateMessageVisualizationStatusForUser(string senderUserId, string messageId, MessageVisualizationStatus status)
//Result(resultStatus, reason) DeleteMessageForUser(string senderUserId, string messageId)


##### Entities #####

--> User
string Id
string Name
string PhoneNumber
string[] ChatIds
string[] BlockedUserIds


--> Chat
string Id
ChatType ChatType
string Name
ChatUser[] Users


--> ChatUser
string UserId
ChatUserRole ChatUserRole


--> Message
string Id
string ChatId
string SenderUserId
string Content
MessageStatus Status


--> ChatType
DirectChat
//GroupChat


--> MessageStatus
//Typing
Sent
//Deleted


--> MessageVisualizationStatus
Sent
//Delivered
//Read


--> ChatUserRole
Administrator
//CommonUser


--> ResultStatus
Success
Failure
Rejected



##### Gherkin #####


--> Cen�rio: 
DADO 
QUANDO 
// E 
ENT�O 

*/