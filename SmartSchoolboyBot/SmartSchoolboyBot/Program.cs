using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// TOKEN телеграм бота
const string Telegram_TOKEN = "7045619439:AAH8oEF-3JAD904pCJYIq8VOwpyE20HxhZI";
bool sentToMode = false;

var botClient = new TelegramBotClient(Telegram_TOKEN);
using var cts = new CancellationTokenSource();

// объект с настройками работы бота (какие типы Update мы будем получать, Timeout бота и так далее)
var receiverOption = new ReceiverOptions
{
    // типы получаемых Update
    AllowedUpdates = new[]
    {
        UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
        UpdateType.CallbackQuery // Inline кнопки
    },

    // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
    // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
    ThrowPendingUpdates = true
};

// запуск бота
botClient.StartReceiving(HandleUpdateAsyns, HandleErrotAsyns, receiverOption, cancellationToken: cts.Token);

// переменная с информацией о боте
var me = await botClient.GetMeAsync();

// задержка для бесприрывной и бесконечной работы бота
await Task.Delay(int.MaxValue);
cts.Cancel();

async Task HandleUpdateAsyns(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    try
    {
        // конструкция switch, чтобы обрабатывать приходящие Update
        switch (update.Type)
        {
            case UpdateType.Message:
                // эта переменная будет содержать в себе все связанное с сообщениями
                var message = update.Message;
                // From - это от кого пришло сообщение (или любой другой Update)
                var user = message!.From;
                // Chat - содержит всю информацию о чате
                var chat = message.Chat;

                // проверка на type сообщения
                switch (message.Type)
                {
                    // обработка текстового type сообщения
                    case MessageType.Text:
                        // обработка команды /start
                        if (message.Text == "/start")
                        {
                            
                        }
                        return;
                    // обработка любых других type сообщений
                    default:
                        await botClient.SendTextMessageAsync(
                            chat.Id,
                            "В данном боте можно использовать только текстовые сообщения",
                            replyToMessageId: message.MessageId);
                        return;
                }
        }    
    }
    catch (Exception)
    {

        throw;
    }
}

Task HandleErrotAsyns(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    // переменная для хранения ошибки и ее кода
    var errorMessege = exception switch
    {
        ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}\n{apiRequestException.Message}]",
        _ => exception.ToString()
    };
    return Task.CompletedTask;
}
