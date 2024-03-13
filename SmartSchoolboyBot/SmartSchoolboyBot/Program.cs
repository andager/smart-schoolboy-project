using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// TOKEN Telegram Bot
const string Telegram_TOKEN = "7045619439:AAH8oEF-3JAD904pCJYIq8VOwpyE20HxhZI";
bool sentToMode = false;

// клиент для работы с Telegram Bot API
var botClient = new TelegramBotClient(Telegram_TOKEN);
// отмена операции
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

    // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда бот был оффлайн
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
            // обработка новых сообщений
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
                        switch (message.Text)
                        {
                            #region [ /start ]
                            // обработка команды [/start]
                            case "/start":

                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    text: $"Приветствую, {user!.FirstName}. Напиши свой номер телефона без пробелов и специальных символов");

                                










                                return;
                            #endregion

                            #region [ Получить расписание ]
                            // обработка команды [Получить расписание]
                            case "Получить расписание":

                                
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Выберите вашу группу:");
                                return;
                            #endregion

                            #region [ Мои достижения ]
                            // обработка команды [Мои достижения]
                            case "Мои достижения":
                                await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Данный функционал в данный момент не работает");
                                return;
                            #endregion

                        }
                        return;
                    // обработка любых других type сообщений
                    default:
                        await botClient.SendTextMessageAsync(chat.Id, "В данном боте можно использовать только текстовые сообщения", replyToMessageId: message.MessageId);
                        return;
                }
            // обработка нажатий на Inline кнопки
            case UpdateType.CallbackQuery:

                return;
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

/*
 *                                 // reply кнопки для выбора действия
                                var actionButton = new ReplyKeyboardMarkup(
                                    // лист (массив), который содрежит в себе массив из класса кнопок
                                    new List<KeyboardButton[]>()
                                    {
                                        // Каждый новый массив - это дополнительные строки
                                        // а каждая дополнительная строка (кнопка) в массиве - это добавление ряда
                                        new KeyboardButton[] // массив кнопок
                                        {
                                            new KeyboardButton(text: "Получить расписание")
                                        },
                                        new KeyboardButton[] // массив кнопок
                                        {
                                            new KeyboardButton(text: "Мои достижения")
                                        }
                                    })
                                {
                                    // автоматическое изменение размера клавиатуры
                                    // True - не изменить, False - (по умолчанию) изменять
                                    ResizeKeyboard = true,

                                    // всегда показывать клавиатуру, когда обычная клавиатура скрыта
                                    // True - показывать, False (по умолчанию) - не показывать
                                    IsPersistent = true
                                };

                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    text: $"{user!.FirstName}, выбери необходимую операцию:",
                                    replyMarkup: actionButton);*/