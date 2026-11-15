-- Cleanup of newsletter/campaign leftovers after the feature was removed from the code base.
-- The NewsLetterSubscription, NewsLetterSubscriptionType and Campaign tables are already dropped;
-- this script only removes the orphan settings, message templates and localization resources.
-- Run once against the target database.

BEGIN TRANSACTION;

-- settings that no longer have a matching property in CustomerSettings / CaptchaSettings / GdprSettings
DELETE FROM [Setting]
WHERE [Name] IN (
    'customersettings.newsletterenabled',
    'customersettings.hidenewsletterblock',
    'customersettings.newsletterblockallowtounsubscribe',
    'customersettings.newslettertickedbydefault',
    'captchasettings.showonnewsletterpage',
    'gdprsettings.lognewsletterconsent'
);

-- message templates of the removed subscription activation flow
DELETE FROM [MessageTemplate]
WHERE [Name] IN (
    'NewsLetterSubscription.ActivationMessage',
    'NewsLetterSubscription.DeactivationMessage'
);

-- localization resources of the removed feature (public site, admin area and permissions)
DELETE FROM [LocaleStringResource]
WHERE [ResourceName] LIKE 'newsletter.%'
   OR [ResourceName] LIKE 'admin.promotions.newslettersubscription%'
   OR [ResourceName] LIKE 'admin.promotions.campaigns%'
   OR [ResourceName] LIKE 'admin.configuration.settings.customeruser.newsletter%'
   OR [ResourceName] LIKE 'admin.configuration.settings.customeruser.hidenewsletterblock%'
   OR [ResourceName] LIKE 'admin.configuration.settings.generalcommon.captchashowonnewsletterpage%'
   OR [ResourceName] LIKE 'admin.configuration.settings.gdpr.lognewsletterconsent%'
   OR [ResourceName] LIKE 'admin.contentmanagement.messagetemplates.description.newslettersubscription%'
   OR [ResourceName] LIKE 'activitylog.%newslettersubscriptions'
   OR [ResourceName] IN (
        'account.subscribetonewsletter',
        'account.fields.newsletter',
        'admin.customers.customers.fields.newsletter',
        'admin.customers.customers.fields.newsletter.hint',
        'gdpr.consent.newsletter',
        'permission.managenewslettersubscribers'
   );

COMMIT TRANSACTION;
