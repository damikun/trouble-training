
export{}

const  incorrect_url = "/some/incorrect/url";

const  correct_url = "https://localhost:5015/hookloopback";

describe('CreateWebhook Tests', () => {

  before(()=>{

    cy.resetdb()
  })

  beforeEach(()=>{
    cy.login("testuser","testuser")

    cy.visit("/Hooks/New")

    cy.contains("Create WebHook")

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/Hooks/New');
  })
  
  //--------------------------------------
  //--------------------------------------

  it('Fill incorrect Url', () => {
    cy.visit("/Hooks/New")

    cy.get('input[name=hookUrl]')
    .type(incorrect_url)
    .should('have.value', incorrect_url);

    cy.get('button[type=submit]')
    .click()

    cy.contains("Invalid URL format");

    cy.get('button[type=submit]')
    .should('be.disabled')

    // Revrite url to correct
    cy.get('input[name=hookUrl]')
    .clear()
    .type(correct_url)
    .should('have.value', correct_url);

    cy.get('button[type=submit]')
    .should('be.enabled')
  });

  //--------------------------------------
  //--------------------------------------

  it('Create WebHook', () => {

    cy.addDummyWebHook(correct_url)

  });

});
