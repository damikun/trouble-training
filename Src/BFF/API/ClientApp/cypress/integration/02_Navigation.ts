
export{}


describe('Navigation Tests', () => {

  beforeEach(()=>{
    cy.login("testuser","testuser")
  })
  
  //--------------------------------------
  //--------------------------------------

  it('Navigate to WebHooks using Route', () => {
    cy.visit("/Hooks")

    cy.contains("WebHook")

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/Hooks');
  });

  it('Navigate WebHook using Tabs', () => {

    cy.get('a[href*="/Hooks"]').click()

    cy.contains("WebHook")

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/Hooks');
  });

  //--------------------------------------
  //--------------------------------------
  
  it('Navigate to CreateHook using Route', () => {
    cy.visit("/Hooks/New")

    cy.contains("Create WebHook")

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/Hooks/New');
  });

  it('Navigate using button CreateNew', () => {

    cy.visit("/Hooks")

    cy.get('button[name=create-new]').click()

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/Hooks/New');
  });

  //--------------------------------------
  //--------------------------------------
  
  it('Navigate to Stream using Tabs', () => {
    cy.get('a[href*="/stream"]').click()

    cy.contains("@Stream")

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/stream');
  });

  it('Navigate to WebHooks using Route', () => {
    cy.visit("/stream")

    cy.contains("@Stream")

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/stream');
  });

  //--------------------------------------
  //--------------------------------------
  
  it('Navigate to StreamDefer using Tabs', () => {
    cy.get('a[href*="/defer+stream"]').click()

    cy.contains("@Stream+@Defer")

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/defer+stream');
  });

  it('Navigate to WebHooks using Route', () => {
    cy.visit("/defer+stream")

    cy.contains("@Stream+@Defer")

    cy.location('pathname', { timeout: 10000 })
    .should('eq', '/defer+stream');
  });
});
