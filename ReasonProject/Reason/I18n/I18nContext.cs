using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;

using Reason.I18n.Context;
using Reason.I18n.FailedMessages;
using Reason.I18n.ExceptionMessages;

namespace Reason.I18n
{
    public abstract class I18nContext
    {
        public I18nContext(CultureInfo culture)
        {
            Culture = culture;
        }

        /// <summary>
        /// The current culture which contains some international properties.
        /// </summary>
        public static I18nContext Current { get; private set; } = CreateContext(CultureInfo.CurrentCulture);

        /// <summary>
        /// A culture of an instance.
        /// </summary>
        public readonly CultureInfo Culture;

        private static I18nContext CreateENGContext()
        {
            return new ContextENG(new CultureInfo("en-US", false));
        }

        /// <summary>
        /// Change a language context for messages which is created by the library.
        /// </summary>
        /// <param name="culture">A language culture.</param>
        public static void ChangeContext(CultureInfo culture)
        {
            Current = CreateContext(culture);
        }

        /// <summary>
        /// Automatic discovery of I18nContext which depends on the input culture under 'Context' directory.
        /// </summary>
        /// <param name="culture">A language culture used for discovery.</param>
        /// <remarks>
        /// <para>
        /// The discovery rules is:<br/>
        /// 1. The class has a namespace of 'Reason.I18n.Context'.<br/>
        /// 2. The class is subclass of 'I18nContext'.<br/>
        /// 3. The class name ends with a string which corresponds to 'Three letter of ISO language name'.<br/>
        /// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.cultureinfo.threeletterisolanguagename?view=net-6.0#system-globalization-cultureinfo-threeletterisolanguagename">Link to MS Docs</seealso>
        /// </para>
        /// </remarks>
        private static I18nContext CreateContext(CultureInfo culture)
        {
            string lang = culture.ThreeLetterISOLanguageName.ToUpper();
            string contextNamespace = typeof(ContextENG).Namespace ?? "Reason.I18n.Context";

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Namespace != contextNamespace) continue;
                if (!type.IsSubclassOf(typeof(I18nContext))) continue;
                if (!type.Name.EndsWith(lang)) continue;

                if (Activator.CreateInstance(type, culture) is not I18nContext context) continue;

                return context;
            }

            return CreateENGContext();
        }

        /// <summary>
        /// Messages for built-in failed reason. 
        /// </summary>
        internal IFailedMessages FailedMessages
        {
            get
            {
                if (failedMessages != null) return failedMessages;

                failedMessages = CreateFailedMessages();

                return failedMessages;
            }
        }
        private IFailedMessages? failedMessages;

        protected virtual IFailedMessages CreateFailedMessages() => new FailedMessagesENG();

        /// <summary>
        /// Messages for built-in exception.
        /// </summary>
        internal IExceptionMessages ExceptionMessages
        {
            get
            {
                if (exceptionMessages != null) return exceptionMessages;

                exceptionMessages = CreateExceptionMessages();

                return exceptionMessages;
            }
        }
        private IExceptionMessages? exceptionMessages;

        protected virtual IExceptionMessages CreateExceptionMessages() => new ExceptionMessagesENG();
    }
}
