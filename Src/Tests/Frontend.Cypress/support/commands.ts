// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add('login', (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })

///////////////////
// Declarations
///////////////////

// cypress/support/index.d.ts


//@ts-ignore
declare namespace Cypress {
  interface Chainable {
    /**
     * Custom command to login using external identity provider
     * @example cy.login('testuser','password')
     */
     login(username: string, password: string, nosession?: boolean): void

    /**
     * Custom command to clear test database
     * @example cy.resetdb()
     */
     resetdb(): void

    /**
     * Custom command to clear test database
     * @example cy.addDummyWebHook("https://localhost/somepath")
     */
     addDummyWebHook(url:string):void
  }
}

///////////////////
// Custom Commands
///////////////////

Cypress.Commands.add('resetdb', () => {

  const options = {
    method: 'POST',
    url: Cypress.config().baseUrl + "/reset",
    body:{},
    headers: {
      "Content-Type": "application/json",
      'X-CSRF': '1'
    },
  };

  cy.request(options).then(
    (response) => {
      expect(response.body).to.eq('Cleared') 
    }
  )
});

//-----------------------------------

Cypress.Commands.add('login', (username, password, nosession = false) => {

    function perform_login(){
      cy.visit('/')

      cy.contains('Login').then(() => {
        
        // Fill UserName
        cy.get('input[name=Username]')
        .type(username)
        .should('have.value', username);

        // Fill Password
        cy.get('input[name=Password]')
        .type(password)
        .should('have.value', password);

        // Confirm login
        cy.get('button[id=login-button]').click()

        // Check redirection to homepage
        cy.location('pathname', { timeout: 10000 }).should('eq', '/');

        cy.wait(100);

        // Check that user is logged-in
        cy.contains(username)
        cy.contains('Welcome')
      });
    }

    if(!nosession){
      cy.session([username, password], () => {
        perform_login();
      })  
    }else{
      perform_login();
    }
    
    // To finish pending animations
    cy.wait(50);

    cy.visit('/')

  })

//-----------------------------------

Cypress.Commands.add('addDummyWebHook', (url) => {

  cy.visit("/Hooks/New")

  cy.contains("Create WebHook")

  cy.get('input[name=hookUrl]')
  .type(url)
  .should('have.value', url);

  cy.get('button[type=submit]')
  .should('be.enabled')
  .click()

  cy.contains(url)
})

  //---------------------------------------

  const COMMAND_DELAY = Cypress.env('COMMAND_DELAY') || 0;

  if (COMMAND_DELAY > 0) {
      for (const command of [
        'visit', 'click', 'trigger',
        'type', 'clear', 'reload',
        'contains']) {
          //@ts-ignore
          Cypress.Commands.overwrite(command, (originalFn, ...args) => {
              const origVal = originalFn(...args);
  
              return new Promise((resolve) => {
                  setTimeout(() => {
                      resolve(origVal);
                  }, COMMAND_DELAY);
              });
          });
      }
  }