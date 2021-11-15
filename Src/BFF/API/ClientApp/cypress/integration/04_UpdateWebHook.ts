
export{}


const  url = "https://localhost:5015/hookloopback2";

const  incorrect_url = "/some/incorrect/url";

describe('UpdateWebhook Tests', () => {

  before(()=>{

    cy.resetdb()
  })

  beforeEach(()=>{
    cy.login("testuser","testuser")

    cy.visit("/Hooks")

    cy.contains("WebHook")

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/Hooks');
  })
  
  //--------------------------------------
  //--------------------------------------
  
  it('Create Test WebHook', () => {

    cy.addDummyWebHook(url)

  });

  //--------------------------------------
  //--------------------------------------

  it('Update WebHook', () => {

    var url_input = "input[name=hookUrl]"
    
    var submit_button = "button[type=submit]"

    //------

    cy.contains("Edit").click()

    cy.contains("Edit WebHook")

    cy.get(url_input)
    .clear();

    cy.get(url_input)
    .type(incorrect_url)
    .should('have.value', incorrect_url);

    cy.get(submit_button)
    .click()

    cy.contains("Invalid URL format");

    cy.get(submit_button)
    .should('be.disabled')

    // Revrite url to correct
    cy.get(url_input)
    .clear()
    .type(url)
    .should('have.value', url)

    cy.get(submit_button)
    .should('be.enabled')
    .click()

    cy.contains("WebHook");

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/Hooks');

    cy.contains(url);
  });



});
