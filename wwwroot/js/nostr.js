async function authenticateWithWindowNostr(challenge) {
    try {
        const publicKey = await window.nostr.getPublicKey();
        const signedEvent = await window.nostr.signEvent({
            created_at: Math.floor(Date.now() / 1000),
            kind: 0,
            tags: [],
            content: challenge,
        });
    } catch (error) {
        console.error("Error authenticating with window.nostr:", error);
    }
}